(* Etec : EqResistanceFRM.fs - Form of the EqResistance Etec module.
 *
 * Copyright (C) 2015 gahag
 * All rights reserved.
 *
 * This software may be modified and distributed under the terms
 * of the BSD license. See the LICENSE file for details.
 *)

namespace Etec.StdModules

  open System.Drawing
  open System.Windows.Forms

  open Etec.Formulas
  open Etec.Base
  open Etec.Base.ModuleUtils
  open Etec.GUI

  [<Sealed>]
  type public EqResistanceFRM() as form =
    inherit Form()

    let mutable PreviousResultMultiplier = UnitMultiplier.z

    let R1LBL           = new Label()
    let R1LBL           = new Label()
    let R2LBL           = new Label()
    let ResultLBL       = new Label()
    let SeriesRB        = new RadioButton()
    let ParallelRB      = new RadioButton()
    let CalculateBTN    = new Button()
    let R1NumericTB     = new NumericTextBox()
    let R2NumericTB     = new NumericTextBox()
    let ResultNumericTB = new NumericTextBox()
    let R1UnitCB        = new UnitComboBox(Ω)
    let R2UnitCB        = new UnitComboBox(Ω)
    let ResultUnitCB    = new UnitComboBox(Ω)
    

    let initControls =
      R1LBL.Text                 <- "R1:"
      R1LBL.Location             <- Point(12, 9)
      R1LBL.Size                 <- Size(24, 13)
      
      R2LBL.Text                 <- "R2:"
      R2LBL.Location             <- Point(12, 35)
      R2LBL.Size                 <- Size(24, 13)

      ResultLBL.Text             <- "Result:"
      ResultLBL.Location         <- Point(12, 84)
      ResultLBL.Size             <- Size(40, 13)

      R1NumericTB.Location       <- Point(42, 6)
      R1NumericTB.Size           <- Size(120, 20)
      R1NumericTB.TabIndex       <- 0

      R1UnitCB.DropDownStyle     <- ComboBoxStyle.DropDownList
      R1UnitCB.Location          <- Point(168, 7)
      R1UnitCB.Size              <- Size(42, 21)
      R1UnitCB.TabIndex          <- 1

      R2NumericTB.Location       <- Point(42, 32)
      R2NumericTB.Size           <- Size(120, 20)
      R2NumericTB.TabIndex       <- 2

      R2UnitCB.DropDownStyle     <- ComboBoxStyle.DropDownList
      R2UnitCB.Location          <- Point(168, 32)
      R2UnitCB.Size              <- Size(42, 21)
      R2UnitCB.TabIndex          <- 3

      SeriesRB.Text              <- "R1 + R2"
      SeriesRB.Location          <- Point(15, 59)
      SeriesRB.Size              <- Size(65, 17)
      SeriesRB.Checked           <- true
      SeriesRB.TabIndex          <- 4

      ParallelRB.Text            <- "R1 // R2"
      ParallelRB.Location        <- Point(141, 59)
      ParallelRB.Size            <- Size(69, 17)
      ParallelRB.TabIndex        <- 5

      CalculateBTN.Text          <- "Calculate"
      CalculateBTN.Location      <- Point(71, 107)
      CalculateBTN.Size          <- Size(75, 23)
      CalculateBTN.TabIndex      <- 6

      ResultUnitCB.DropDownStyle <- ComboBoxStyle.DropDownList
      ResultUnitCB.Location      <- Point(168, 81)
      ResultUnitCB.Size          <- Size(42, 21)
      ResultUnitCB.TabIndex      <- 7

      ResultNumericTB.Location   <- Point(58, 81)
      ResultNumericTB.Size       <- Size(104, 20)
      ResultNumericTB.ReadOnly   <- true
      ResultNumericTB.TabIndex   <- 8



    do
      form.SuspendLayout()
      initControls
          
      List.iter form.Controls.Add 
        [ R1UnitCB        :> Control
        ; R1LBL           :> Control
        ; R1LBL           :> Control
        ; R2LBL           :> Control
        ; SeriesRB        :> Control
        ; ParallelRB      :> Control
        ; CalculateBTN    :> Control
        ; ResultLBL       :> Control
        ; R1NumericTB     :> Control
        ; R2NumericTB     :> Control
        ; ResultNumericTB :> Control
        ; R1UnitCB        :> Control
        ; R2UnitCB        :> Control
        ; ResultUnitCB    :> Control ]

      form.StartPosition   <- FormStartPosition.CenterScreen
      form.FormBorderStyle <- FormBorderStyle.FixedToolWindow
      form.ClientSize      <- new Size(222, 137)
      form.Text            <- "Equivalent Resistance"


      R1NumericTB.KeyPress.Add
      <| fun e -> if e.KeyChar = char Keys.Enter
                    then R2NumericTB.Focus() |> ignore

      R2NumericTB.KeyPress.Add
      <| fun e -> if e.KeyChar = char Keys.Enter
                  && R2NumericTB.CheckValue()
                    then form.CalculateBTN_Click()

      CalculateBTN.Click.Add(ignore >> form.CalculateBTN_Click)

      ResultUnitCB.SelectedValueChanged.Add(ignore >> 
        form.ResultUnitCB_SelectedValChanged)


      form.ResumeLayout(false)
      form.PerformLayout()



    member form.CalculateBTN_Click() =
      let computation = if SeriesRB.Checked
                          then Impedance.Resistance.seriesEq
                          else Impedance.Resistance.parallelEq

      let (value, multiplier) =
        resetMultiplier <| computation (R1NumericTB.Value * R1UnitCB.Value)
                                        (R2NumericTB.Value * R2UnitCB.Value)

      do ResultUnitCB.SelectedValue <- multiplier
         ResultNumericTB.Value      <- value


    member form.ResultUnitCB_SelectedValChanged() = do
      ResultNumericTB.Value <- setMultiplier
                                (ResultNumericTB.Value *
                                  multiplierVal PreviousResultMultiplier)
                                ResultUnitCB.SelectedValue

      PreviousResultMultiplier <- ResultUnitCB.SelectedValue
