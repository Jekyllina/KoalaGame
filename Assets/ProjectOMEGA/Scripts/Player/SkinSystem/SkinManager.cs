using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    [SerializeField] private List<Skin> skins;

    public Skin currentSkin { get; private set; }

    public void SetSkin(string skinID)
    {
        Skin skinToSet = FindSkin(skinID);

        if (skinToSet != null)
        {
            currentSkin = skinToSet;

            foreach (Skin skin in skins)
            {
                if (skin.GetID() == skinID)
                    skin.gameObject.SetActive(true);
                else
                    skin.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogError($"{skinID} doesn't exists!");
        }
    }

    private Skin FindSkin(string skinID)
    {
        foreach (var skin in skins)
        {
            if (skin.GetID() == skinID)
                return skin;
        }

        return null;
    }


}
