// Copyright (c) Nate McMaster.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if FEATURE_UNLOAD
namespace TSharp.Bundle
{
  /// <summary>
  ///   Represents the method that will handle the <see cref="BundleLoader.Reloaded" /> event.
  /// </summary>
  /// <param name="sender">The object sending the event</param>
  /// <param name="eventArgs">Data about the event.</param>
  public delegate void BundleReloadedEventHandler(object sender, BundleReloadedEventArgs eventArgs);

  /// <summary>
  ///   Provides data for the <see cref="BundleLoader.Reloaded" /> event.
  /// </summary>
  public class BundleReloadedEventArgs : EventArgs
  {
    /// <summary>
    ///   Initializes <see cref="BundleReloadedEventArgs" />.
    /// </summary>
    /// <param name="loader"></param>
    public BundleReloadedEventArgs(BundleLoader loader)
    {
      Loader = loader;
    }

    /// <summary>
    ///   The plugin loader
    /// </summary>
    public BundleLoader Loader { get; }
  }
}
#endif