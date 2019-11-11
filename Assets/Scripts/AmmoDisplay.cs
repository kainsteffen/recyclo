using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplay : MonoBehaviour
{
    PlayerController player;
    public Image ammo1;
    public Image ammo2;
    public Image ammo3;

    // Start is called before the first frame update
    private void Awake()
    {
        player = GameController.Instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        ammo1.fillAmount = 1 - (3f - player.currentAmmo);
        ammo2.fillAmount = 2 - (3f - player.currentAmmo);
        ammo3.fillAmount = 3 - (3f - player.currentAmmo);
    }
}
