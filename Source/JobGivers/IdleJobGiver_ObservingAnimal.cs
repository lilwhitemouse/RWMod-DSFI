﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using RimWorld;

namespace DSFI.JobGivers
{
    public class IdleJobGiver_ObservingAnimal : IdleJobGiver<IdleJobGiverDef_ObservingAnimal>
    {
        public override float GetWeight(Pawn pawn, Trait traitIndustriousness)
        {
            if (pawn.story.WorkTypeIsDisabled(WorkTypeDefOf.Handling))
            {
                return 0f;
            }

            if (pawn.story.WorkTagIsDisabled(WorkTags.Animals))
            {
                return 0f;
            }

            return base.GetWeight(pawn, traitIndustriousness);
        }

        static HashSet<Pawn> pawns = new HashSet<Pawn>();
        public override Job TryGiveJob(Pawn pawn)
        {
            if (!JoyUtility.EnjoyableOutsideNow(pawn.Map))
            {
                return null;
            }

            pawns.Clear();
            foreach (Thing thing in GenRadial.RadialDistinctThingsAround(pawn.Position, pawn.Map, this.def.searchDistance, true))
            {
                Pawn targetPawn = thing as Pawn;
                if (targetPawn != null && targetPawn.RaceProps.Animal)
                {
                    pawns.Add(targetPawn);
                }
            }

            if (pawns.Any())
            {
                Pawn target = pawns.RandomElement();
                return new Job(IdleJobDefOf.IdleJob_ObservingAnimal, target)
                {
                    locomotionUrgency = LocomotionUrgency.Walk
                };
            }

            return null;
        }
    }
}
