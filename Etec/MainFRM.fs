(* Etec : MainFRM.fs - Etec's main form.
 *
 * Copyright (C) 2015 gahag
 * All rights reserved.
 *
 * This software may be modified and distributed under the terms
 * of the BSD license. See the LICENSE file for details.
 *)

namespace Etec
  
  open System
  open System.Drawing
  open System.Windows.Forms
  
  open Etec.Base
  
  
  [<Sealed>]
  type public MainFRM(Modules : IEtecModule list) as form =
    inherit Form()
  
    let ModuleBtnHeight = 25;
    let ModuleBtnWidth  = 75;
    let ModuleBtnMargin = 10;
  
    let line i = int <| Math.Ceiling(double i / 4.0)
    let column i = match i % 4 with
                   | 0 -> 4
                   | c -> c
  
    let top i = ModuleBtnHeight * (line i - 1)
              + ModuleBtnMargin * line i
    let left i = ModuleBtnWidth  * (column i - 1)
               + ModuleBtnMargin * column i
    
    do
      form.SuspendLayout();
  
      for i in 1 .. Modules.Length
        do let Module = List.nth Modules (i - 1)
             
           let btn = new Button()
           btn.Text     <- Module.Name
           btn.Location <- Point(left i, top i)
           btn.Size     <- Size(ModuleBtnWidth, ModuleBtnHeight)
           btn.Click.Add(fun _ -> Module.Run) // Ignore the EventArgs.
            
           form.Controls.Add(btn)
        
  
      form.StartPosition       <- FormStartPosition.CenterScreen
      form.FormBorderStyle     <- FormBorderStyle.FixedToolWindow
      form.AutoScaleDimensions <- SizeF(6.0f, 13.0f)
      form.AutoScaleMode       <- AutoScaleMode.Font
      form.Text                <- "Etec - Made by gahag"
      form.ClientSize
        <- new Size (
            350,
            ModuleBtnMargin * (line Modules.Length + 1)
          + ModuleBtnHeight * line Modules.Length
           )
  
      form.ResumeLayout(false)
      form.PerformLayout()
