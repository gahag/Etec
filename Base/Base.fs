(* Etec : Base.fs - Base interfaces, types and functions for Etec.
 *
 * Copyright (C) 2015 gahag
 * All rights reserved.
 *
 * This software may be modified and distributed under the terms
 * of the BSD license. See the LICENSE file for details.
 *)

namespace Etec.Base

  open System
  open System.Windows.Forms

  [<Interface>]
  type IEtecModule =
    abstract member Name : String // Name of the module.
    abstract member Form : Form   // Module form.

  type Unit = Ω // Ohm
            | A // Ampere
            | V // Volt
            | F // Farad
            | H // Henri
    with override this.ToString() =
          match this with
          | Ω -> "Ω"
          | A -> "A"
          | V -> "V"
          | F -> "F"
          | H -> "H"

  // The values represent the exponent in a base 10 exponentiation.
  type UnitMultiplier = T =  12 // Tera 
                      | G =  9  // Giga
                      | M =  6  // Mega
                      | K =  3  // Kilo
                      | z =  0  // zero
                      | m = -3  // mili
                      | µ = -6  // micro
                      | n = -9  // nano
                      | p = -12 // pico

  module ModuleUtils =
    
    // Get the value of a multiplier.
    // UnitMultiplier -> Float
    let multiplierVal m = Math.Pow(10.0, double m)

    // Apply a multiplier to a value.
    // Float -> UnitMultiplier -> Float
    let setMultiplier v m = v / multiplierVal m

    // Apply the appropriate multiplier to a value.
    // Float -> Float * UniMultiplier
    let rec resetMultiplier = function
      | 0.0 -> (0.0, UnitMultiplier.z)

      | v when v < 0.0 -> (fun (v', m) -> -v', m)
                       <| resetMultiplier -v

      | v when v >= multiplierVal UnitMultiplier.K
          -> (fun (v', m) -> (v', m + UnitMultiplier.K))
          <| resetMultiplier (v * multiplierVal UnitMultiplier.m)

      | v when v < multiplierVal UnitMultiplier.z
          -> (fun (v', m) -> (v', m + UnitMultiplier.m))
          <| resetMultiplier (v * multiplierVal UnitMultiplier.K)

      | v' -> (v', UnitMultiplier.z)
