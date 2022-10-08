using HarmonyLib;
using Verse;

namespace SimpleCultivation
{
    [HarmonyPatch(typeof(Thing), "TakeDamage")]
    public static class Thing_TakeDamage_Patch
    {
        public static bool preventRecursion;
        public static void Prefix(Thing __instance, DamageInfo dinfo)
        {
            if (dinfo.Instigator is Pawn attacker)
            {
                bool isMeleeAttack = dinfo.Weapon is null || dinfo.Weapon == attacker.def
                    || dinfo.Weapon.IsMeleeWeapon;
                if (isMeleeAttack)
                {
                    if (attacker.health.hediffSet.GetFirstHediffOfDef(SC_DefOf.SC_CoreFormation) is Hediff_CoreFormation hediff)
                    {
                        hediff.AddProgress(0.001f);
                    }
                }
            }
        }
    }
}
