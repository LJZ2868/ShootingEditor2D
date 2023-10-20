using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootEditor2D
{
    public class Platform : MonoBehaviour
    {
        private void OnTriggerExit2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
                collision.isTrigger = false;
        }
    }
}