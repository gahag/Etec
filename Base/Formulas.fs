(* Etec : Formulas.fs - Standard formulas for Etec modules.
 *
 * Copyright (C) 2015 gahag
 * All rights reserved.
 *
 * This software may be modified and distributed under the terms
 * of the BSD license. See the LICENSE file for details.
 *)

namespace Etec.Formulas

  open System
  
  module private Math =
      let sum = (+) : Double -> Double -> Double
      let recSum x y = (x * y) / (x + y) : Double // Reciprocal sum.
      let pi2 = 2.0 * Math.PI


  open Math

  module Impedance =
    module Resistance =
      let seriesEq   = sum
      let parallelEq = recSum

    module Reatance =
      let capacitive f c = 1.0 / (f * c * pi2)
      let inductive  f i = f * i * pi2


  module Capacitance =
    let seriesEq   = recSum
    let parallelEq = sum


  module Inductance =
    let seriesEq   = sum
    let parallelEq = recSum
