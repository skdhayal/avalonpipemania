using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using AvalonPipeMania.Code.Extensions;
using AvalonPipeMania.Assets.Shared;

namespace AvalonPipeMania.Code.Assets
{
	[Script]
	public static class KnownAnimations
	{
		public static AnimationCollection Emoticons = 
			new AnimationCollection(prefix => KnownAssets.Default.FileNames.Where(k => k.StartsWith(prefix)).OrderBy(k => k).ToArray())
			{
			   // { 60, 32, 32, KnownAssets.Path.Animation.pengsuitani},
			    { 60, 37, 18, KnownAssets.Path.Animation.weekend_feeling},
			    { 70, 50, 50, KnownAssets.Path.Animation.irritating_fly},
			    { 70, 40, 22, KnownAssets.Path.Animation.troll},
			    { 50, 50, 30, KnownAssets.Path.Animation.pancakeglomp},
			    { 70, 48, 25, KnownAssets.Path.Animation.five_seconds_hug},
			    { 20, 40, 40, KnownAssets.Path.Animation.duck_ride},
			    { 60, 50, 50, KnownAssets.Path.Animation.daily_deviation},
			    { 60, 44, 40, KnownAssets.Path.Animation.bubbles_time},
			    { 60, 40, 20, KnownAssets.Path.Animation.clone}
			};
	}
}
