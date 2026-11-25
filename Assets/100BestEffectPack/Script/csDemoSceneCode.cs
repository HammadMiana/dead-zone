using UnityEngine;
using TMPro;

public class csDemoSceneCode : MonoBehaviour
{
    public string[] EffectNames;
    public string[] Effect2Names;
    public Transform[] Effect;
    public TMP_Text text1;

    int index = 0;

    void Start()
    {
        SpawnEffect();
    }

    void Update()
    {
        text1.text = (index + 1) + " : " + EffectNames[index];

        if (Input.GetKeyDown(KeyCode.Z))
            ChangeIndex(-1);

        if (Input.GetKeyDown(KeyCode.X))
            ChangeIndex(1);

        if (Input.GetKeyDown(KeyCode.C))
            SpawnEffect();
    }

    void ChangeIndex(int direction)
    {
        index += direction;

        if (index < 0)
            index = Effect.Length - 1;
        else if (index >= Effect.Length)
            index = 0;

        SpawnEffect();
    }

    void SpawnEffect()
    {
        bool isGroundEffect = false;

        foreach (string name in Effect2Names)
        {
            if (EffectNames[index] == name)
            {
                isGroundEffect = true;
                break;
            }
        }

        Vector3 pos = isGroundEffect ? new Vector3(0, 0.2f, 0) : new Vector3(0, 5f, 0);
        Instantiate(Effect[index], pos, Quaternion.identity);
    }
}
