using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shield : MonoBehaviour
{
    [SerializeField] Image shieldImg;
    [SerializeField] Sprite spShield1;
    [SerializeField] Sprite spShield2;

    float cooldownAnimation = 0.33f;
    float baseCooldownAnimation;
    bool sprite1 = true;

	private void Start()
	{
        baseCooldownAnimation = cooldownAnimation;
        shieldImg.sprite = spShield1;
    }

	void Update()
    {
        if (GameManager.instance.GetGameState() == GameManager.GameState.GAME || GameManager.instance.GetGameState() == GameManager.GameState.ALIEN_WAVE)
            MoveElement();

        SpriteAnimation();
    }

    public void MoveElement()
    {
        if (!CharacterManager.instance.GetHasVortex())
            gameObject.transform.position += new Vector3(0, GameManager.instance.GetScrolingSpeed(), 0) * Time.deltaTime;
        else
        {
            Vector3 toCharacterVector = CharacterManager.instance.GetCharacterPosition() - gameObject.transform.position;
            gameObject.transform.position += new Vector3(0, GameManager.instance.GetScrolingSpeed(), 0) * Time.deltaTime;
            gameObject.transform.position += (toCharacterVector.normalized * CharacterManager.instance.GetVortexAttractionSpeed()) * Time.deltaTime;
        }
    }

    void SpriteAnimation()
	{
        if (cooldownAnimation <= 0)
        {
            if (sprite1)
                shieldImg.sprite = spShield2;
            else
                shieldImg.sprite = spShield1;

            sprite1 = !sprite1;

            cooldownAnimation = baseCooldownAnimation;
        }
        else
            cooldownAnimation -= Time.deltaTime;
	}
}
