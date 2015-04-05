(* Etec : GUI.fs - Standard GUI components for Etec modules.
 *
 * Copyright (C) 2015 gahag
 * All rights reserved.
 *
 * This software may be modified and distributed under the terms
 * of the BSD license. See the LICENSE file for details.
 *)

namespace Etec.GUI
  
  open System
  open System.Linq
  open System.Windows.Forms

  open Etec.Base
  open Etec.Base.ModuleUtils


  type NumericTextBox() =
    inherit TextBox()

    let SigChars = [ '+'; '-' ]
    let DecChars = [ '.'; ',' ]
    let NumChars = [ '1'; '2'; '3'
                   ; '4'; '5'; '6'
                   ; '7'; '8'; '9'
                   ; ' '; '0'     ]
    
    member this.RawVal
      with get() = this.Text.Replace(" ", String.Empty)
    
    member this.Value
      with get() = match this.Text with
                    | "" -> 0.0
                    | _  -> Double.Parse this.RawVal
      and set(v:float) = this.Text <- string v

    
    member this.CheckValue() : bool =
      let error s =
        MessageBox.Show(s, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

      let checkVal = this.Text = String.Empty
                  || try Double.Parse this.RawVal |> ignore; true
                     with | :? System.FormatException -> false
      
      match checkVal with
      | true -> true
      | false -> error "Please inform a valid value!" |> ignore
                 this.Focus() |> ignore
                 false


    override this.OnKeyPress e = do
      let c = e.KeyChar

      let contains (s:String) (cs:char list) = s.Any <| Func<_,_> cs.Contains
      let isDec = DecChars.Contains
      let isNum = NumChars.Contains
      let isSig = SigChars.Contains

      base.OnKeyPress e

      if not (c = char Keys.Back)
      && not (isSig c
              && this.SelectionStart = 0
              && not (this.Text |>contains<| SigChars))
      && not (isDec c
              && this.SelectionStart > 0
              && not (this.Text |>contains<| DecChars)
              && isNum this.Text.[this.SelectionStart - 1])
      && not (isNum c)
        then e.Handled <- true

    override this.OnLeave e = do
      base.OnLeave e
      this.CheckValue() |> ignore



  type UnitComboBox(u : Unit) as this =
    inherit ComboBox()

    do let unit = string u
              
       this.Items.AddRange(
        Array.ConvertAll (
          Array.sortWith
            (fun (a:UnitMultiplier) b -> -a.CompareTo b)
          <| downcast Enum.GetValues(typeof<UnitMultiplier>)
          ,
          fun i -> if i = UnitMultiplier.z
                    then upcast unit
                    else upcast (string i + unit)
        )
       )

       this.DropDownStyle <- ComboBoxStyle.DropDownList
       this.SelectedItem <- unit
       

    member this.Unit = u

    member this.Value
      with get() =
        let Selected = string this.SelectedItem

        if Selected = string this.Unit
          then 1.0
          else multiplierVal
            <| downcast
                Enum.Parse(typeof<UnitMultiplier>, string Selected.[0])

    member this.SelectedValue
      with get() =
        let Selected = string this.SelectedItem
                   
        if Selected = string this.Unit
          then UnitMultiplier.z
          else downcast
                Enum.Parse(typeof<UnitMultiplier>, string Selected.[0])

      and set(v) =
        this.SelectedItem <-
          if v = UnitMultiplier.z
            then string this.Unit
            else string v + string this.Unit
