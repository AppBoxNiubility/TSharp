// Copyright (c) Nate McMaster.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace TSharp.Bundle.Internal;

internal class RuntimeOptions
{
  public string[]? AdditionalProbingPaths { get; set; }
  public string? Tfm { get; set; }
}