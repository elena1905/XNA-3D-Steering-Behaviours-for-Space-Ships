using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Steering
{
    class EliteFighter:Fighter
    {
        private static Random random = new Random(DateTime.Now.Millisecond);
            
        public static string[] models = { "adder", "anaconda", "asp", "boa", "cobramk1", "cobramk3", "ferdelance", "gecko", "krait", "marimba", "moray", "python", "shuttle", "sidewinder", "transporter", "viper" };
        public EliteFighter()
            : base()
        {
            //ModelName = models[random.Next() % models.Count()];
            ModelName = models[3];
            look = new Vector3(SteeringBehaviours.RandomClamped()
                 , SteeringBehaviours.RandomClamped()
                , SteeringBehaviours.RandomClamped()
                );
            look.Normalize();

        }
    }
}
