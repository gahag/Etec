(* Etec : OhmLawFRM.fs - Form of the OhmLaw Etec module.
 *
 * Copyright (C) 2015 gahag
 * All rights reserved.
 *
 * This software may be modified and distributed under the terms
 * of the BSD license. See the LICENSE file for details.
 *)

namespace Etec.StdModules

  open System
  open System.Drawing
  open System.Windows.Forms

  open Etec.Formulas
  open Etec.Base
  open Etec.Base.ModuleUtils
  open Etec.GUI

  type OhmLawVals<'i> = VR of 'i * 'i
                      | RI of 'i * 'i
                      | VI of 'i * 'i

  [<Sealed>]
  type public OhmLawFRM() as form =
    inherit Form()

    let VNumericTB = new NumericTextBox()
    let RNumericTB = new NumericTextBox()
    let INumericTB = new NumericTextBox()
    let VUnitCB    = new UnitComboBox(V)
    let RUnitCB    = new UnitComboBox(Ω)
    let IUnitCB    = new UnitComboBox(A)
    let VLockRB    = new RadioButton()
    let RLockRB    = new RadioButton()
    let ILockRB    = new RadioButton()
    

    let initControls =

      VNumericTB.Location <- Point(6, 6)
      VNumericTB.Size     <- Size(120, 20)
      VNumericTB.TabIndex <- 0

      VUnitCB.Location    <- Point(132, 6)
      VUnitCB.Size        <- Size(42, 21)
      VUnitCB.TabIndex    <- 1

      VLockRB.Location    <- Point(180, 6)
      VLockRB.Size        <- Size(12, 18)
      VLockRB.TabIndex    <- 2
      VLockRB.Checked     <- true

      RNumericTB.Location <- Point(6, 32)
      RNumericTB.Size     <- Size(120, 20)
      RNumericTB.TabIndex <- 3

      RUnitCB.Location    <- Point(132, 32)
      RUnitCB.Size        <- Size(42, 21)
      RUnitCB.TabIndex    <- 4

      RLockRB.Location    <- Point(180, 32)
      RLockRB.Size        <- Size(12, 18)
      RLockRB.TabIndex    <- 5

      INumericTB.Location <- Point(6, 58)
      INumericTB.Size     <- Size(120, 20)
      INumericTB.TabIndex <- 6

      IUnitCB.Location    <- Point(132, 58)
      IUnitCB.Size        <- Size(42, 21)
      IUnitCB.TabIndex    <- 7

      ILockRB.Location    <- Point(180, 58)
      ILockRB.Size        <- Size(12, 18)
      ILockRB.TabIndex    <- 8



    do
      form.SuspendLayout()
      initControls
          
      List.iter form.Controls.Add 
        [ VNumericTB :> Control
        ; RNumericTB :> Control
        ; INumericTB :> Control
        ; VUnitCB    :> Control
        ; RUnitCB    :> Control
        ; IUnitCB    :> Control
        ; VLockRB    :> Control
        ; RLockRB    :> Control
        ; ILockRB    :> Control ]

      form.StartPosition   <- FormStartPosition.CenterScreen
      form.FormBorderStyle <- FormBorderStyle.FixedToolWindow
      form.ClientSize      <- new Size(200, 84)
      form.Text            <- "Ohm Law"

      VNumericTB.TextChanged.Add(
        fun _ -> if RLockRB.Checked
                  then INumericTB.Value <- VNumericTB.Value / RNumericTB.Value
                  else RNumericTB.Value <- VNumericTB.Value / INumericTB.Value
      )

      RNumericTB.TextChanged.Add(
        fun _ -> if VLockRB.Checked
                  then INumericTB.Value <- VNumericTB.Value / RNumericTB.Value
                  else VNumericTB.Value <- INumericTB.Value * RNumericTB.Value
      )

      INumericTB.TextChanged.Add(
        fun _ -> if VLockRB.Checked
                  then RNumericTB.Value <- VNumericTB.Value / INumericTB.Value
                  else VNumericTB.Value <- RNumericTB.Value * INumericTB.Value
      )

      form.ResumeLayout(false)
      form.PerformLayout()
