(* Etec : Program.fs - Etec's entry point and module loading.
 *
 * Copyright (C) 2015 gahag
 * All rights reserved.
 *
 * This software may be modified and distributed under the terms
 * of the BSD license. See the LICENSE file for details.
 *)

namespace Etec

  open System
  open System.Windows.Forms
  open System.Reflection

  open Etec.Base

  module Main =
  
    // MonadTransformer : Either + List
    type EitherList<'a, 'b> = Left  of 'a
                            | Right of 'b list
  
    let (<@>) el f = // fmap only for the either layer
      match el with
      | Right l -> Right (f l)
      | Left  l -> Left l

    let mzero = Right [] // mzero for EitherList

    let sequence = List.fold ( // sequence for EitherList
                    fun s t -> match (s, t) with
                               | (Left _, _) -> s
                               | (_, Left _) -> t
                               | (Right xs, Right x) -> Right (x@xs)
                   ) mzero


    let (>>=) el f = // Bind for EitherList
      match el with
      | Right l -> l |> List.map f |> sequence
      | Left l  -> Left l

    let return' x = Right <| Seq.toList x // kind of a return, except it returns
                                          // from a sequence instead of a single
                                          // element

    let filter f el = el <@> List.filter f // filter for EitherList

    let map f el = el <@> List.map f // map for EitherList


    let LoadModules dlls =
      let activate t = downcast Activator.CreateInstance(t : Type)
      let isEtecModule (t : Type) =
          t.GetInterface(typeof<IEtecModule>.FullName) <> null
       && not t.IsInterface
       && not t.IsAbstract

      return' dlls >>=
      (AssemblyName.GetAssemblyName
      >> Assembly.Load
      >> fun asm -> map activate << filter isEtecModule
                      <| try return' <| asm.GetTypes()
                         with | _ -> Left <| "File: " + asm.Location)



    [<STAThread>]
    [<EntryPoint>]
    do let error s =
        MessageBox.Show (
          "Fatal error: failed to load:\n" + s,
          "Fatal error",
          MessageBoxButtons.OK,
          MessageBoxIcon.Error
        )
      
       let dlls =
        System.IO.Directory.GetFiles(
          AppDomain.CurrentDomain.BaseDirectory,
          "*.dll"
        )
  

       Application.EnableVisualStyles()
       Application.SetCompatibleTextRenderingDefault(false)
       match LoadModules dlls with
        | Left err -> error err |> ignore
        | Right ms -> Application.Run(new MainFRM(ms) :> Form)
