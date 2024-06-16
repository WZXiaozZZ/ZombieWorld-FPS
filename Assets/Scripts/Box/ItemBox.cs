using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public enum BoxType
    {
        None,
        Bullet,
        Hp,
    }
    [SerializeField] private BoxType boxType = BoxType.None;
    [SerializeField] private int addValue = 50;
    [SerializeField] private Transform lid;
    private bool isOpen;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isOpen)
            return;

        lid.Rotate(Time.fixedDeltaTime * -70, 0, 0);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Defines.PlayerTag))
        {
            switch (boxType)
            {
                case BoxType.None:
                    break;
                case BoxType.Bullet:
                    Player.Instance.AddBullet(addValue);
                    break;
                case BoxType.Hp:
                    Player.Instance.AddHP(addValue);
                    break;
                default:
                    break;
            }
            isOpen = true;
            Destroy(gameObject,2f);
        }
    }

}
