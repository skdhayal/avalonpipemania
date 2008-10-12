using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared;
using ScriptCoreLib.ActionScript;


namespace AvalonPipeMania.Assets
{
	namespace Shared
	{

		[Script]
		public class KnownAssets : AssetsImplementationDetails
		{
			public static readonly KnownAssets Default = new KnownAssets();

			[Script, ScriptResources]
			public static class Path
			{
				public const string Assets = "assets/AvalonPipeMania.Assets";
				public const string Data = "assets/AvalonPipeMania.Data";
				public const string Sounds = "assets/AvalonPipeMania.Sounds";
				public const string Explosion = "assets/AvalonPipeMania.Explosion";

				[Script, ScriptResources]
				public static class Fonts
				{
					public const string showcard = "assets/AvalonPipeMania.Fonts/showcard";


				}

				[Script, ScriptResources]
				public static class Animation
				{
					public const string pengsuitani = "assets/AvalonPipeMania.Animation/pengsuitani";
					public const string weekend_feeling = "assets/AvalonPipeMania.Animation/weekend_feeling";
					public const string irritating_fly = "assets/AvalonPipeMania.Animation/irritating_fly";
					public const string troll = "assets/AvalonPipeMania.Animation/troll";
					public const string pancakeglomp = "assets/AvalonPipeMania.Animation/pancakeglomp";
					public const string five_seconds_hug = "assets/AvalonPipeMania.Animation/five_seconds_hug";
					public const string duck_ride = "assets/AvalonPipeMania.Animation/duck_ride";
					public const string daily_deviation = "assets/AvalonPipeMania.Animation/daily_deviation";
					public const string bubbles_time = "assets/AvalonPipeMania.Animation/bubbles_time";
					public const string clone = "assets/AvalonPipeMania.Animation/clone";
					

				}

				[Script, ScriptResources]
				public static class Pipe
				{
					public const string LeftToRight = "assets/AvalonPipeMania.Pipe/LeftToRight";
					public const string LeftToRightBent = "assets/AvalonPipeMania.Pipe/LeftToRightBent";
					public const string LeftToRightWide = "assets/AvalonPipeMania.Pipe/LeftToRightWide";
					public const string LeftToDrain = "assets/AvalonPipeMania.Pipe/LeftToDrain";
					public const string TopToBottom = "assets/AvalonPipeMania.Pipe/TopToBottom";
					public const string TopToBottomWide = "assets/AvalonPipeMania.Pipe/TopToBottomWide";
					public const string RightToBottom = "assets/AvalonPipeMania.Pipe/RightToBottom";
					public const string TopToRight = "assets/AvalonPipeMania.Pipe/TopToRight";
					public const string PumpToLeft = "assets/AvalonPipeMania.Pipe/PumpToLeft";
					public const string PumpHandle = "assets/AvalonPipeMania.Pipe/PumpHandle";
					public const string PumpToRight = "assets/AvalonPipeMania.Pipe/PumpToRight";
					public const string LeftToBottom = "assets/AvalonPipeMania.Pipe/LeftToBottom";
					public const string TopToLeft = "assets/AvalonPipeMania.Pipe/TopToLeft";
					public const string RightToDrain = "assets/AvalonPipeMania.Pipe/RightToDrain";
					public const string Missing = "assets/AvalonPipeMania.Pipe/Missing";

					// yellow:
					// hue +32
					// sat +154

					// green:
					// hue +92
				}
			}


		}

		public class AssetsImplementationDetails
		{
			// This class has the native implementation
			// JavaScript and ActionScript have their own implementations!

			public string[] FileNames
			{
				get
				{
					return ScriptCoreLib.CSharp.Extensions.EmbeddedResourcesExtensions.GetEmbeddedResources(null, this.GetType().Assembly);
				}
			}

		}

	}

	namespace JavaScript
	{
		[Script(Implements = typeof(Shared.AssetsImplementationDetails))]
		internal class __AssetsImplementationDetails
		{
			public string[] FileNames
			{
				[EmbedGetFileNames]
				get
				{
					throw new NotImplementedException();
				}
			}
		}
	}

	namespace ActionScript
	{
		[Script(Implements = typeof(Shared.AssetsImplementationDetails))]
		internal class __AssetsImplementationDetails
		{
			public string[] FileNames
			{
				[EmbedGetFileNames]
				get
				{
					throw new NotImplementedException();
				}
			}

		}


	}
}
