using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private float lifetime = 1.5f;
    [SerializeField] private TextMeshPro text;

    private void Start()
    {
        //Destroys the GO after set lifetime value
        StartCoroutine("DestroyGO");
    }

    private void LateUpdate()
    {
        transform.LookAt(GameManager.inst.gpManager.player.transform);
    }

    public void InitDamageIndicator(int damage, Transform enemyTransform, Color textColor)
    {
        //Initialize and set damge indicator's position and parent
        gameObject.transform.parent = enemyTransform;
        gameObject.transform.localPosition = new Vector3(Random.Range(-0.3F,0.3F), Random.Range(0.8f, 1f), 0f);
        text.text = damage.ToString();
        text.color = textColor;

        //Do the animation
        gameObject.transform.DOLocalMoveY(2f, 1.5f);
        text.DOFade(0f, 0.5f);
    }
    private IEnumerator DestroyGO()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
