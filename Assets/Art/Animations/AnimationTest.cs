using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{
	Animator myAnim;
    // Start is called before the first frame update
    void Start()
    {
		myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.A))
		{
			myAnim.SetTrigger("Attack");
		}

		if (Input.GetKeyDown(KeyCode.D))
			myAnim.SetTrigger("Hurt");

		if (Input.GetKeyDown(KeyCode.Space))
			myAnim.SetTrigger("Skill");

		if (Input.GetKeyDown(KeyCode.W))
			myAnim.SetBool("Move", true);

		if (Input.GetKeyDown(KeyCode.S))
			myAnim.SetBool("Move", false);

		if (Input.GetKeyDown(KeyCode.X))
			myAnim.SetBool("Dead", true);

	}
}
