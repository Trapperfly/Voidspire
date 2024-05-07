using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ExtensionMethods;
using Unity.Transforms;
using UnityEngine.UIElements;

public class PointerSystem : MonoBehaviour
{
    public CameraController cController;
    Transform player;
    public Transform compass;
    Transform pingLoc;
    bool active;
    public TMP_Text distanceText;
    public float compassRotateSpeed;
    //public Transform hud;
    //float radius;
    //public List<Transform> pings = new();
    //public GameObject textPrefab;
    //public List<TMP_Text> texts = new();
    //Color onColor = Color.white;
    //Color offColor = new(1,1,1,0);
    //[Range(0.001f, 0.02f)]
    //public float pointerSpeed = 0.02f;
    //float opacity = 0;

    public float bossHints;

    Vector3 direction;
    Quaternion lookDir;

    #region Singleton
    public static PointerSystem Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    //public ActiveTarget target;
    void Start()
    {
        player = GlobalRefs.Instance.player.transform;
        ResetCompass();
    }

    private void Update()
    {
        if (!active) {
            return; 
        }
        Vector2 direction = (pingLoc.position - player.position).normalized;
        float angle = Vector2.SignedAngle(Vector2.up, direction);
        compass.rotation = Quaternion.RotateTowards(compass.rotation, Quaternion.Euler(0, 0, angle), compassRotateSpeed);
        //lookDir = Quaternion.LookRotation(direction);
        //compass.rotation = Quaternion.Slerp(compass.rotation, lookDir, Time.deltaTime * 0.5f);
        distanceText.text = Mathf.Clamp(Vector2.Distance(player.position, pingLoc.position) / 10, 1, 999).ToString("F0");
    }

    public void AddPing(Transform pos)
    {
        pingLoc = pos;
        active = true;
    }

    public void ResetCompass()
    {
        bossHints = 0;
        distanceText.text = new("???");
        compass.rotation = new(0, 0, 0, 0);
        active = false;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.P))
    //    {
    //        opacity = 10;
    //    }
    //    if (pings.Count < 1)
    //    {
    //        Debug.Log("Pings were empty");
    //        return;
    //    }
    //    for (int i = 0; i < pings.Count; i++)
    //    {
    //        if (!pings[i]) pings.Remove(pings[i]);
    //    }
    //    if (pings.Count == texts.Count) { }
    //    else if (pings.Count > texts.Count) for (int i = 0; i < pings.Count - texts.Count; i++) 
    //        {
    //            TMP_Text newText = Instantiate(textPrefab, player.position, new Quaternion(), hud).GetComponent<TMP_Text>();
    //            texts.Add(newText);
    //        } 
    //    else if (pings.Count < texts.Count) for (int i = 0; i < texts.Count - pings.Count; i++)
    //        {
    //            Destroy(texts[pings.Count + i].gameObject);
    //            texts.RemoveAt(pings.Count + i);
    //        }
    //    radius = cController.currentZoom * 0.5f;
    //    for (int i = 0; i < pings.Count; i++)
    //    {
    //        Vector2 _direction = pings[i].position - player.position;
    //        _direction.Normalize();
    //        Vector2 pos = (Vector2)player.position + _direction * radius;
    //        texts[i].transform.position = Vector2.Lerp(texts[i].transform.position, new(pos.x, pos.y), pointerSpeed) ;
    //        //texts[i].transform.position = new(pos.x, pos.y);
    //        texts[i].transform.localScale = Vector3.one;
    //        float _dist = Vector2.Distance(player.transform.position, pings[i].position);

    //        if (_dist < 2 * radius)
    //        {
    //            texts[i].color = Vector4.Lerp(offColor, onColor, (_dist - radius) / radius);
    //        }
    //        else if (_dist < 8 * radius)
    //        {
    //            texts[i].color = Vector4.Lerp(offColor, onColor, ((8*radius) - _dist));
    //        }
    //        else
    //            texts[i].color = new(1,1,1,opacity);
    //        texts[i].text = pings[i].name + "\n";
    //        if (_dist * 0.1f < 0.95f) texts[i].text += (_dist * 0.1f).ToString("F1") + "km";
    //        else texts[i].text += (_dist * 0.1f).ToString("F0") + "km";

    //        if (_dist < radius) RemovePing(pings[i]);
    //    }
    //    opacity -= Time.deltaTime;
    //}

    //public void RemovePing(Transform ping)
    //{
    //    pings.Remove(ping);
    //}

    //public void AddPing(Vector2 pos)
    //{
    //    GameObject pingGO = new(name = "New Ping");
    //    pingGO.transform.position = pos;
    //    Transform ping = pingGO.transform;
    //    pings.Add(ping);
    //    opacity = 5;
    //}
    //public void AddPing(Transform pos)
    //{
    //    pings.Add(pos);
    //    opacity = 5;
    //}
    //public void TestAddPing()
    //{
    //    if (target.targetRB) {
    //        AddPing(target.target);
    //    } else if (target.target) AddPing(target.target.position);
    //    //Vector2 pos = (Vector2)player.transform.position + new Vector2(Random.Range(-1000,1000),Random.Range(-1000,1000));
    //}
}
