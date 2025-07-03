using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Test
{
	public class Turn_Move : MonoBehaviour
	{
		public float turnX;
		public float turnY;
		public float turnZ;

		public float moveX;
		public float moveY;
		public float moveZ;

		public bool world;

		// Use this for initialization
		void Start()
		{

		}

		//Update is called once per frame
		void Update()
		{
			if (world == true)
			{
				transform.Rotate(turnX * Time.deltaTime, turnY * Time.deltaTime, turnZ * Time.deltaTime, Space.World);
				transform.Translate(moveX * Time.deltaTime, moveY * Time.deltaTime, moveZ * Time.deltaTime, Space.World);
			}
			else
			{
				transform.Rotate(turnX * Time.deltaTime, turnY * Time.deltaTime, turnZ * Time.deltaTime, Space.Self);
				transform.Translate(moveX * Time.deltaTime, moveY * Time.deltaTime, moveZ * Time.deltaTime, Space.Self);
			}
		}
	}
}
