<<<<<<< HEAD
﻿using DNAI.Cars2;
=======
﻿//using DNAI.DemoCar2;
>>>>>>> machine_learning
using UnityEngine;

namespace Assets.Scripts
{
<<<<<<< HEAD
    public class CarTest : Cars2
=======
    public class CarTest : MonoBehaviour// DemoCar2
>>>>>>> machine_learning
    {
        public float minDistance;
        public float speed = 10f;

        //public CarController left;
        //public CarController right;
        //private AI carai = new AI();

        private Material mat;

        // Use this for initialization
        void Start()
        {
<<<<<<< HEAD
            carai.minDistance = minDistance;
            carai.speed = speed;
=======
            //carai.LeftCaptor = new Captor();
            //carai.RightCaptor = new Captor();
            //carai.minDistance = minDistance;
            //carai.speed = speed;
>>>>>>> machine_learning
            //mat = GetComponent<MeshRenderer>().material;
        }

        // Update is called once per frame
        void Update()
        {
<<<<<<< HEAD
            carai.leftCaptorDistance = minDistance;
            carai.rightCaptorDistance = minDistance;
            carai.minDistance = minDistance;

            long val = (long)carai.GetDirection(carai);
            switch (val)
            {
                case (long)Direction.FORWARD:
                    Debug.Log("forward" + val.ToString());
                    transform.Translate(Vector3.forward * speed * Time.deltaTime);
                    break;
                case (long)Direction.LEFT:
                    Debug.Log("left" + val.ToString());
                    //transform.Rotate(Vector3.up * (-angularSpeed) * Time.deltaTime);
                    transform.Translate(Vector3.forward * speed * Time.deltaTime);
                    break;
                case (long)Direction.RIGHT:
                    Debug.Log("right" + val.ToString());
                    //transform.Rotate(Vector3.up * angularSpeed * Time.deltaTime);
                    transform.Translate(Vector3.forward * speed * Time.deltaTime);
                    break;
                default:
                    Debug.Log("stop" + val.ToString());
                    break;
            }
=======
            //carai.UpdateDirection(carai);
            //transform.Translate(new Vector3((float)carai.X, (float)carai.Y, (float)carai.Z) * Time.deltaTime);
>>>>>>> machine_learning
        }
    }
}