(* Etec : UnitComboBox.fs - Standard unit combo box for Etec modules.
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
