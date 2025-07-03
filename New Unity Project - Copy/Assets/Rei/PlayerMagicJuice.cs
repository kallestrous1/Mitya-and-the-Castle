using UnityEngine;
using UnityEngine.UI;

public class PlayerMagicJuice : MonoBehaviour
{
    public Slider magicSlider;
    public static int maxMagic = 10;
    public int currentMagic = 10;


    void Start()
    {
        currentMagic = maxMagic;
        magicSlider.maxValue = maxMagic;
        magicSlider.value = currentMagic;
    }

    public void changeMagic(int amount)
    {
        currentMagic += amount;
        magicSlider.value = currentMagic;

    }
}
