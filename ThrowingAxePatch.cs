using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace ThrowingAxeBalanceMod
{
    [HarmonyPatch(typeof(DefaultItemValueModel),"CalculateTierMeleeWeapon")]
    public class ThrowingAxePatch
    {
        static bool Prefix(WeaponComponent weaponComponent, DefaultItemValueModel __instance, ref float __result) {
            float num = float.MinValue;
            float num2 = float.MinValue;
            for (int i = 0; i < weaponComponent.Weapons.Count; i++)
            {
                WeaponComponentData weaponComponentData = weaponComponent.Weapons[i];
                var getFactor = AccessTools.Method(typeof(DefaultItemValueModel), "GetFactor");
                float a = (float)weaponComponentData.ThrustDamage * (float)getFactor.Invoke(__instance, new object[] { weaponComponentData.ThrustDamageType }) * MathF.Pow((float)weaponComponentData.ThrustSpeed * 0.01f, 1.5f);
                float num3 = (float)weaponComponentData.SwingDamage * (float)getFactor.Invoke(__instance, new object[] { weaponComponentData.SwingDamageType}) * MathF.Pow((float)weaponComponentData.SwingSpeed * 0.01f, 1.5f);
                float num4 = MathF.Max(a, num3 * 1.1f);
                if (weaponComponentData.WeaponFlags.HasAnyFlag(WeaponFlags.NotUsableWithOneHand))
                {
                    num4 *= 0.8f;
                }
                if (weaponComponentData.WeaponClass == WeaponClass.ThrowingKnife)
                {
                    num4 *= 1.2f;
                }
                if (weaponComponentData.WeaponClass == WeaponClass.ThrowingAxe) {
                    num4 *= 0.7f;
                }
                if (weaponComponentData.WeaponClass == WeaponClass.Javelin)
                {
                    num4 *= 0.6f;
                }
                float num5 = (float)weaponComponentData.WeaponLength * 0.01f;
                float num6 = 0.06f * (num4 * (1f + num5)) - 3.5f;
                if (num6 > num2)
                {
                    if (num6 >= num)
                    {
                        num2 = num;
                        num = num6;
                    }
                    else
                    {
                        num2 = num6;
                    }
                }
            }
            num = MathF.Clamp(num, -1.5f, 7.5f);
            if (num2 != -3.4028235E+38f)
            {
                num2 = MathF.Clamp(num2, -1.5f, 7.5f);
            }
            if (weaponComponent.Weapons.Count <= 1)
            {
                __result = num;
                return false;
            }
            __result = num * MathF.Pow(1f + (num2 + 1.5f) / (num + 2.5f), 0.2f);
            return false;
        }
    }
}
