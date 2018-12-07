using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using NativeUI;

namespace vMenuClient
{
    public class WeatherOptions
    {
        // Variables
        private UIMenu menu;
        private CommonFunctions cf = MainMenu.Cf;
        public static Dictionary<string, UIMenuItem> weatherHashMenuIndex = new Dictionary<string, UIMenuItem>();
        public UIMenuCheckboxItem dynamicWeatherEnabled;
        public UIMenuCheckboxItem blackout;

        private void CreateMenu()
        {
            // Create the menu.
            menu = new UIMenu(GetPlayerName(Game.Player.Handle), "Weather Options", true)
            {
                ScaleWithSafezone = false,
                MouseControlsEnabled = false,
                MouseEdgeEnabled = false,
                ControlDisablingEnabled = false
            };

            dynamicWeatherEnabled = new UIMenuCheckboxItem("Toggle Dynamic Weather", EventManager.dynamicWeather, "Enable or disable dynamic weather changes.");
            blackout = new UIMenuCheckboxItem("Toggle Blackout", EventManager.blackoutMode, "This disables or enables all lights across the map.");
            UIMenuItem extrasunny = new UIMenuItem("Extra Sunny", "Set the weather to ~y~extra sunny~s~!");
            UIMenuItem clear = new UIMenuItem("Clear", "Set the weather to ~y~clear~s~!");
            UIMenuItem neutral = new UIMenuItem("Neutral", "Set the weather to ~y~neutral~s~!");
            UIMenuItem smog = new UIMenuItem("Smog", "Set the weather to ~y~smog~s~!");
            UIMenuItem foggy = new UIMenuItem("Foggy", "Set the weather to ~y~foggy~s~!");
            UIMenuItem clouds = new UIMenuItem("Cloudy", "Set the weather to ~y~clouds~s~!");
            UIMenuItem overcast = new UIMenuItem("Overcast", "Set the weather to ~y~overcast~s~!");
            UIMenuItem clearing = new UIMenuItem("Clearing", "Set the weather to ~y~clearing~s~!");
            UIMenuItem rain = new UIMenuItem("Rainy", "Set the weather to ~y~rain~s~!");
            UIMenuItem thunder = new UIMenuItem("Thunder", "Set the weather to ~y~thunder~s~!");
            UIMenuItem blizzard = new UIMenuItem("Blizzard", "Set the weather to ~y~blizzard~s~!");
            UIMenuItem snow = new UIMenuItem("Snow", "Set the weather to ~y~snow~s~!");
            UIMenuItem snowlight = new UIMenuItem("Light Snow", "Set the weather to ~y~light snow~s~!");
            UIMenuItem xmas = new UIMenuItem("X-MAS Snow", "Set the weather to ~y~x-mas~s~!");
            UIMenuItem halloween = new UIMenuItem("Halloween", "Set the weather to ~y~halloween~s~!");
            UIMenuItem removeclouds = new UIMenuItem("Remove All Clouds", "Remove all clouds from the sky!");
            UIMenuItem randomizeclouds = new UIMenuItem("Randomize Clouds", "Add random clouds to the sky!");

            var indexOffset = 2;
            if (cf.IsAllowed(Permission.WODynamic))
            {
                menu.AddItem(dynamicWeatherEnabled);
                indexOffset--;
            }
            if (cf.IsAllowed(Permission.WOBlackout))
            {
                menu.AddItem(blackout);
                indexOffset--;
            }
            if (cf.IsAllowed(Permission.WOSetWeather))
            {
                weatherHashMenuIndex.Add("-1750463879", extrasunny);
                weatherHashMenuIndex.Add("916995460", clear);
                weatherHashMenuIndex.Add("-1530260698", neutral);
                weatherHashMenuIndex.Add("282916021", smog);
                weatherHashMenuIndex.Add("-1368164796", foggy);
                weatherHashMenuIndex.Add("821931868", clouds);
                weatherHashMenuIndex.Add("-1148613331", overcast);
                weatherHashMenuIndex.Add("1840358669", clearing);
                weatherHashMenuIndex.Add("1420204096", rain);
                weatherHashMenuIndex.Add("-1233681761", thunder);
                weatherHashMenuIndex.Add("669657108", blizzard);
                weatherHashMenuIndex.Add("-273223690", snow);
                weatherHashMenuIndex.Add("603685163", snowlight);
                weatherHashMenuIndex.Add("-1429616491", xmas);
                weatherHashMenuIndex.Add("-921030142", halloween);

                menu.AddItem(extrasunny);
                menu.AddItem(clear);
                menu.AddItem(neutral);
                menu.AddItem(smog);
                menu.AddItem(foggy);
                menu.AddItem(clouds);
                menu.AddItem(overcast);
                menu.AddItem(clearing);
                menu.AddItem(rain);
                menu.AddItem(thunder);
                menu.AddItem(blizzard);
                menu.AddItem(snow);
                menu.AddItem(snowlight);
                menu.AddItem(xmas);
                menu.AddItem(halloween);
            }
            if (cf.IsAllowed(Permission.WORandomizeClouds))
            {
                menu.AddItem(removeclouds);
            }

            if (cf.IsAllowed(Permission.WORemoveClouds))
            {
                menu.AddItem(randomizeclouds);
            }

            List<string> weatherTypes = new List<string>()
            {
                "EXTRASUNNY",
                "CLEAR",
                "NEUTRAL",
                "SMOG",
                "FOGGY",
                "CLOUDS",
                "OVERCAST",
                "CLEARING",
                "RAIN",
                "THUNDER",
                "BLIZZARD",
                "SNOW",
                "SNOWLIGHT",
                "XMAS",
                "HALLOWEEN"
            };

            menu.OnItemSelect += (sender, item, index2) =>
            {
                var index = index2 + indexOffset;
                // A weather type is selected.
                if (index >= 2 && index <= 16)
                {
                    Notify.Custom($"The weather will be changed to ~y~{weatherTypes[index - 2]}~s~ in the next 45 seconds.");
                    cf.UpdateServerWeather(weatherTypes[index - 2], EventManager.blackoutMode, EventManager.dynamicWeather);
                }
                if (item == removeclouds)
                {
                    cf.ModifyClouds(true);
                }
                else if (item == randomizeclouds)
                {
                    cf.ModifyClouds(false);
                }
            };

            menu.OnCheckboxChange += (sender, item, _checked) =>
            {
                if (item == dynamicWeatherEnabled)
                {
                    EventManager.dynamicWeather = _checked;
                    Notify.Custom($"Dynamic weather changes are now {(_checked ? "~g~enabled" : "~r~disabled")}~s~.");
                    cf.UpdateServerWeather(EventManager.currentWeatherType, EventManager.blackoutMode, _checked);
                }
                else if (item == blackout)
                {
                    EventManager.blackoutMode = _checked;
                    Notify.Custom($"Blackout mode is now {(_checked ? "~g~enabled" : "~r~disabled")}~s~.");
                    cf.UpdateServerWeather(EventManager.currentWeatherType, _checked, EventManager.dynamicWeather);
                }
            };
        }



        /// <summary>
        /// Create the menu if it doesn't exist, and then returns it.
        /// </summary>
        /// <returns>The Menu</returns>
        public UIMenu GetMenu()
        {
            if (menu == null)
            {
                CreateMenu();
            }
            return menu;
        }
    }
}
