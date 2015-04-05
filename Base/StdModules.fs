(* Etec : StdModules.fs - Standard modules for Etec.
 *
 * Copyright (C) 2015 gahag
 * All rights reserved.
 *
 * This software may be modified and distributed under the terms
 * of the BSD license. See the LICENSE file for details.
 *)

namespace Etec.StdModules
  
  open System.Windows.Forms

  open Etec.Base

  [<Sealed>]
  type EqResistance() =
    interface IEtecModule with
      member this.Name = "Resistor Eq"
      member this.Form = new EqResistanceFRM() :> Form
  
  [<Sealed>]
  type OhmLaw() =
    interface IEtecModule with
      member this.Name = "Ω Law"
      member this.Form = new OhmLawFRM() :> Form
