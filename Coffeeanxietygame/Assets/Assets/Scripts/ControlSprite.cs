﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlSprite : MonoBehaviour
{
    public WordHolder words;
    public SpriteRenderer bar;
    public Sprite[] bars;
    public Sprite[] faces;
    public SpriteRenderer face;
    public float speed;
    public int hp;
    private int confidencelevel;
    private Rigidbody2D rigid;
    private SpriteRenderer sprite;
    private bool invincible;
    private float iframes;
    private string level;
    void Start(){
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        invincible = false;
        face.sprite = faces[hp];
        level = LevelManager.Level;
        confidencelevel = 0;
        if (level=="three"){
            bar.sprite = bars[3];
        }
        else{
            bar.sprite = bars[confidencelevel];
        }
        
    }

    void Update(){
        if (hp==-1){
            LevelManager.Level = "loser";
            SceneManager.LoadScene(1);
        }
        
    }

    void UpdateBar(){
        if (level=="three"){
            switch (confidencelevel){
                case 2:
                    bar.sprite = bars[2];
                    break;
                case 4:
                    bar.sprite = bars[1];
                    break;
                case 6:
                    bar.sprite = bars[0];
                    break;
            }
        }
        else if (confidencelevel<4){
            bar.sprite = bars[confidencelevel];
        }
    }

    void OnCollisionEnter2D(Collision2D collision){
        if (!invincible && collision.gameObject.tag=="badend"){
            hp--;
            face.sprite = faces[hp];
            invincible = true;
            iframes = 2.0f;
            StartCoroutine(InvincibilityFrames());
            collision.gameObject.SetActive(false);
            confidencelevel++;
            UpdateBar();
        }
        else if(collision.gameObject.tag=="goodend"){
            if (words.checkwon()){
                switch (level){
                    case "tutorial":
                        LevelManager.Level = "tutorialwin";
                        SceneManager.LoadScene(1);
                        break;
                    case "one":
                        LevelManager.Level = "onewin";
                        SceneManager.LoadScene(1);
                        break;
                    case "two":
                        LevelManager.Level = "twowin";
                        SceneManager.LoadScene(1);
                        break;
                }
            }
            else{
                collision.gameObject.SetActive(false);
                words.incrementcurrent(level);
            }
        }
        else if(collision.gameObject.tag=="confidence"){
            confidencelevel++;
            if (confidencelevel==8){
                LevelManager.Level = "threewin";
                SceneManager.LoadScene(1);
            }
            else{
                UpdateBar();
                words.generateConfidence();
            }
        }
    }

    void FixedUpdate(){
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        rigid.velocity = new Vector2(moveHorizontal*speed, moveVertical*speed);
    }

    IEnumerator InvincibilityFrames(){
        while (invincible){
            yield return new WaitForSeconds(0.2f);
            iframes-=0.2f;
            if (iframes<=0.0f){
                invincible = false;
            }
            Flicker();
        }
        sprite.enabled = true;
    }

    void Flicker(){
        sprite.enabled = !sprite.isVisible;
    }
}
