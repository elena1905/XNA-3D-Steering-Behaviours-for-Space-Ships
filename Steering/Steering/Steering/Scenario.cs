using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Steering
{
    class Scenario
    {
        static Random random = new Random(DateTime.Now.Millisecond);

        static Vector3 randomPosition(float range)
        {
            Vector3 pos = new Vector3();
            pos.X = (random.Next() % range) - (range / 2);
            pos.Y = (random.Next() % range) - (range / 2);
            pos.Z = (random.Next() % range) - (range / 2);
            return pos;
        }

        public static void setUpFlockingDemo()
        {
            Params.Load("flocking.properties");
            List<Entity> children = XNAGame.Instance().Children;
            
            // Create an EliteFighter instance to be the pursuer
            
            float range = Params.GetFloat("world_range");

            Fighter fighter = new Fighter();
            fighter.ModelName = "ship2";
            //fighter.pos = new Vector3(10.0f, 10.0f, 10.0f);
            //fighter.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.pursuit);
            

            // Create Params.GetFloat("num_boids") boids and turn on the appropriate steering behaviours..
            float boids = Params.GetFloat("num_boids");

            for (int i = 0; i < boids;  i++)
            {
                Fighter boid = new Fighter();
                boid.ModelName = "fighter";
                boid.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.alignment);
                boid.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.cohesion);
                boid.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.wander);
                boid.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.sphere_constrain);
                boid.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.separation);

                // use Vector3 pos = randomPosition(range); to generate random positions
                Vector3 pos = randomPosition(range);
                boid.pos = pos;
                //fighter.targetPos = boid.pos + new Vector3(-50, 0, -80);
                children.Add(boid);
            }
            //children.Add(fighter);

            int numObstacles = 5;
            float dist = (range * 2) / numObstacles;
            for (float x = - range ; x < range ; x+= dist)
            {
                for (float z = - range ; z < range ; z += dist)
                {
                    Obstacle o = new Obstacle(20);
                    o.pos = new Vector3(x, 0, z);
                    o.Color = new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
                    o.ShouldDraw = true;
                    children.Add(o);
                }
            }
            
            // Create a camera fighter
            // Give it a "Leader" to follow and turn on offset pursuit
            //XNAGame.Instance().CamFighter = XXX?
            Camera camera = XNAGame.Instance().Camera;
            camera.pos = new Vector3(0.0f, 60.0f, 200.0f);
        }

        public static void setUpStateMachineDemo()
        {
            List<Entity> children = XNAGame.Instance().Children;            
            Ground ground = new Ground();
            children.Add(ground);
            XNAGame.Instance().Ground = ground;            
            AIFighter aiFighter = new AIFighter();
            aiFighter.pos = new Vector3(-20, 50, 50);
            aiFighter.maxSpeed = 16.0f;
            aiFighter.SwicthState(new IdleState(aiFighter));
            aiFighter.Path.DrawPath = true;
            children.Add(aiFighter);

            Fighter fighter = new Fighter();
            fighter.ModelName = "ship2";
            fighter.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.arrive);
            fighter.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.obstacle_avoidance);
            fighter.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.wall_avoidance);
            fighter.pos = new Vector3(10, 50, 0);
            fighter.targetPos = aiFighter.pos + new Vector3(-50, 0, -80);
            children.Add(fighter);

            Fighter camFighter = new Fighter();
            camFighter.Leader = fighter;            
            camFighter.offset = new Vector3(0, 5, 10);
            camFighter.pos = fighter.pos + camFighter.offset;
            camFighter.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.offset_pursuit);
            camFighter.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.wall_avoidance);
            camFighter.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.obstacle_avoidance);
            XNAGame.Instance().CamFighter = camFighter;
            children.Add(camFighter);

            XNAGame.Instance().Leader = fighter;
            Camera camera = XNAGame.Instance().Camera;
            camera.pos = new Vector3(0.0f, 60.0f, 100.0f);

            Obstacle o = new Obstacle(4);
            o.pos = new Vector3(0, 50, -10);
            children.Add(o);

            o = new Obstacle(4);
            o.pos = new Vector3(50, 0, -90) + aiFighter.pos;
            children.Add(o);


        }

        public static void setUpPursuit()
        {
            List<Entity> children = XNAGame.Instance().Children;

            Ground ground = new Ground();
            children.Add(ground);
            XNAGame.Instance().Ground = ground;            

            Fighter fighter = new Fighter();
            fighter.ModelName = "ship1";
            fighter.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.arrive);
            fighter.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.obstacle_avoidance);
            fighter.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.wall_avoidance);
            fighter.pos = new Vector3(2, 20, -50);
            fighter.targetPos = fighter.pos * 2;
            XNAGame.Instance().Leader = fighter;
            children.Add(fighter);

            Fighter fighter1 = new Fighter();
            fighter1.ModelName = "ship2";
            fighter1.Target = fighter;
            fighter1.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.pursuit);
            fighter1.pos = new Vector3(-20, 20, -20);
            children.Add(fighter1);                        
        }

        public static void setUpWander()
        {
            List<Entity> children = XNAGame.Instance().Children;
            Fighter leader = new Fighter();
            leader.pos = new Vector3(10, 120, 20);
            leader.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.wander);
            leader.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.obstacle_avoidance);
            leader.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.wall_avoidance);
            children.Add(leader);

            Fighter camFighter = new Fighter();
            camFighter.Leader = leader;
            camFighter.pos = new Vector3(10, 120, 0);
            camFighter.offset = new Vector3(0, 5, 10);
            camFighter.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.offset_pursuit);
            camFighter.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.wall_avoidance);
            camFighter.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.obstacle_avoidance);
            XNAGame.Instance().CamFighter = camFighter;
            children.Add(camFighter);

            Ground ground = new Ground();
            children.Add(ground);
            XNAGame.Instance().Ground = ground;

            XNAGame.Instance().Camera.pos = new Vector3(10, 120, 50);            
      
        }


        public static void setUpArrive()
        {
            List<Entity> children = XNAGame.Instance().Children;
            Fighter leader = new Fighter();
            leader.pos = new Vector3(10, 20, 20);
            leader.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.arrive);
            leader.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.obstacle_avoidance);
            leader.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.wall_avoidance);
            leader.targetPos = new Vector3(0, 100, -450);
            children.Add(leader);
            XNAGame.Instance().Leader = leader;
            Ground ground = new Ground();
            children.Add(ground);
            XNAGame.Instance().Ground = ground;
            foreach (Entity child in children)
            {
                child.pos.Y += 100;
            }
        }
        

        public static void setUpBuckRogersDemo()
        {
            List<Entity> children = XNAGame.Instance().Children;
            Fighter leader = new Fighter();
            leader.pos = new Vector3(10, 20, 20);            
            leader.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.seek);
            leader.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.obstacle_avoidance);
            leader.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.wall_avoidance);
            leader.targetPos = new Vector3(0, 100, -450);
            children.Add(leader);
            XNAGame.Instance().Leader = leader;

            // Add some Obstacles

            Obstacle o = new Obstacle(4);
            o.pos = new Vector3(0, 10, -10);
            children.Add(o);

            o = new Obstacle(17);
            o.pos = new Vector3(-10, 16, -80);
            children.Add(o);

            o = new Obstacle(10);
            o.pos = new Vector3(10, 15, -120);
            children.Add(o);

            o = new Obstacle(12);
            o.pos = new Vector3(5, -10, -150);
            children.Add(o);

            o = new Obstacle(20);
            o.pos = new Vector3(-2, 5, -200);
            children.Add(o);

            o = new Obstacle(10);
            o.pos = new Vector3(-25, -20, -250);
            children.Add(o);

            o = new Obstacle(10);
            o.pos = new Vector3(20, -20, -250);
            children.Add(o);

            o = new Obstacle(35);
            o.pos = new Vector3(-10, -30, -300);
            children.Add(o);

            // Now make a fleet
            int fleetSize = 5;
            float xOff = 6;
            float zOff = 6;
            for (int i = 2; i < fleetSize; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    float z = (i - 1) * +zOff;
                    Fighter fleet = new Fighter();
                    fleet.Leader = leader;
                    fleet.offset = new Vector3((xOff * (-i / 2.0f)) + (j * xOff), 0, z);
                    fleet.pos = leader.pos + fleet.offset;
                    fleet.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.offset_pursuit);
                    fleet.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.wall_avoidance);
                    fleet.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.obstacle_avoidance);
                    children.Add(fleet);
                }
            }

            Fighter camFighter = new Fighter();
            camFighter.Leader = leader;
            camFighter.pos = new Vector3(0, 15, fleetSize * zOff);
            camFighter.offset = new Vector3(0, 5, fleetSize * zOff);
            camFighter.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.offset_pursuit);
            camFighter.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.wall_avoidance);
            camFighter.SteeringBehaviours.turnOn(SteeringBehaviours.behaviour_type.obstacle_avoidance);
            XNAGame.Instance().CamFighter = camFighter;
            children.Add(camFighter);


            Ground ground = new Ground();
            children.Add(ground);
            XNAGame.Instance().Ground = ground;
            foreach (Entity child in children)
            {
                child.pos.Y += 100;
            }
        }
    }
}
