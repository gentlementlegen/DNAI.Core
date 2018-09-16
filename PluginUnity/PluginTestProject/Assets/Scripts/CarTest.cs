using DNAI.DemoCar2;
using UnityEngine;

namespace Assets.Scripts
{
    public class CarTest : DemoCar2
    {
        public float minDistance;
        public float speed = 10f;

        //public CarController left;
        //public CarController right;
        private AI carai = new AI();

        private Material mat;

        // Use this for initialization
        void Start()
        {
            carai.LeftCaptor = new Captor();
            carai.RightCaptor = new Captor();
            carai.minDistance = minDistance;
            carai.speed = speed;
            //mat = GetComponent<MeshRenderer>().material;
        }

        // Update is called once per frame
        void Update()
        {
            carai.UpdateDirection(carai);
            transform.Translate(new Vector3((float)carai.X, (float)carai.Y, (float)carai.Z) * Time.deltaTime);
        }
    }
}