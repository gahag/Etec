(* Etec : StdModules.fs - Standard modules for Etec.
 *
 * Copyright (C) 2015 gahag
 * All rights reserved.
 *
 * This software may be modified and distributed under the terms
 * of the BSD license. See the LICENSE file for details.
 *)

#nowarn "0760"  // Forms dispose themselves, no need for explicit `new`.

namespace Etec.StdModules
  
  open Etec.Base

  [<Sealed>]
  type EqResistance() =
    interface IEtecModule with
      member this.Name = "Resistor Eq"
      member this.Run  = EqResistanceFRM().Show // The action is just showing the form.
  
  [<Sealed>]
  type OhmLaw() =
    interface IEtecModule with
      member this.Name = "Ω Law"
      member this.Run  = OhmLawFRM().Show // The action is just showing the form.
