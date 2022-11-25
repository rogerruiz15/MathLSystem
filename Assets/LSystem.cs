using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;
using TMPro;

public class Trans
{
    public Vector3 position;
    public Quaternion rotation;

}


public class LSystem : MonoBehaviour
{
    public Transform spawn;
    public Toggle toggle;
    public Dropdown dropdown;
    public GameObject toggEnabled;
    private int preAngle = 0;
    private float originalAngle;
    private int doOnce = 0;

    [SerializeField] private int itinerations = 5;
    [SerializeField] private GameObject branch;
    [SerializeField] private float length = 7;
    [SerializeField] private float angle = 25.7f;

    private Stack<Trans> transformStack;
    private Dictionary<char, string> rules;
    public float randomvalue1 = 0;
    public float randomvalue2 = 0;

    private string currentString = string.Empty;

     
    private void Update()
    {
        if (toggle.isOn)
        {
            originalAngle = angle;
            dropdown.interactable = false;
            toggEnabled.SetActive(true);
            doOnce = 1;
        }
        else
        {
            
            dropdown.interactable = true;
            toggEnabled.SetActive(false);
            if (doOnce == 1)
            {
                angle = originalAngle;
                Generate();
                doOnce = 0;
            }
           

        }

    }

    public void Generate()
    {
        DestroyObjects();
        transform.position = spawn.position;
        transform.rotation = spawn.rotation;
        
        if (rules.Count == 1)
        {
            currentString = "F";
        }
        else
        {
            currentString = "X";
        }

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < itinerations; i++)
        {
            foreach (char c in currentString)
            {
                sb.Append(rules.ContainsKey(c) ? rules[c] : c.ToString());
                
            }

            currentString = sb.ToString();
            sb = new StringBuilder();
            
        }

        
        Debug.Log(currentString);
        foreach (char c in currentString)
        {
            switch (c)
            {
                case 'F':
                    Vector3 initialPos = transform.position;
                    transform.Translate(Vector3.up * length);
                    GameObject segment = Instantiate(branch, spawn);
                    segment.GetComponent<LineRenderer>().SetPosition(0, initialPos);
                    segment.GetComponent<LineRenderer>().SetPosition(1, transform.position);
                    
                    break;
                case 'X':
                    break;
                case '+':
                    RandomAngle();
                    transform.Rotate(Vector3.back * angle);
                    break;
                case '-':
                    RandomAngle();
                    transform.Rotate(Vector3.forward * angle);
                    break;
                case '[':
                    transformStack.Push(new Trans()
                    {
                        position = transform.position,
                        rotation = transform.rotation
                    });
                    
                    break;
                case ']':
                    Trans ti = transformStack.Pop();
                    transform.position = ti.position;
                    transform.rotation = ti.rotation;
                    break;

                default:
                    throw new InvalidOperationException("Invalid L-System");

            }
        }

    }

    public void SetItineration(int val)
    {
        itinerations = val + 1;
        Debug.Log(val);
        DestroyObjects();
        Generate();
    }

    public void SetRule(int val)
    {
        Debug.Log(val);
        if (val == 0)
        {
            transformStack = new Stack<Trans>();
            rules = new Dictionary<char, string>{
            {'F', "F[+F]F[-F]F" }
            };
            angle = 25.7f;
            length = 7;
            itinerations = 5;
            Generate();
        }
        else if (val == 1)
        {
            transformStack = new Stack<Trans>();
            rules = new Dictionary<char, string>{
            {'F', "F[+F]F[-F][F]" }
            };
            angle = 20f;
            length = 7;
            itinerations = 5;
            Generate();
        }
        else if (val == 2)
        {
            transformStack = new Stack<Trans>();
            rules = new Dictionary<char, string>{
            
            {'F', "FF-[-F+F+F]+[+F-F-F]" }
            };
            angle = 22.5f;
            length = 7;
            itinerations = 4;
            Generate();
        }
        else if (val == 3)
        {
            transformStack = new Stack<Trans>();
            rules = new Dictionary<char, string>{
            {'X', "F[+X]F[-X]X" },
            {'F', "FF" }
            };
            angle = 20f;
            length = 1;
            itinerations = 7;
            Generate();
        }
        
        else if (val == 4)
        {
            transformStack = new Stack<Trans>();
            rules = new Dictionary<char, string>{
            {'X', "F[+X][-X]FX" },
            {'F', "FF" }
            };
            angle = 25.7f;
            length = 1;
            itinerations = 7;
            Generate();
        }
        else if (val == 5)
        {
            transformStack = new Stack<Trans>();
            rules = new Dictionary<char, string>{
            {'X', "F-[[X]+X]+F[+FX]-X" },
            {'F', "FF" }
            };
            angle = 22.5f;
            length = 5;
            itinerations = 5;
            Generate();
        }
        else if (val == 6)
        {
            transformStack = new Stack<Trans>();
            rules = new Dictionary<char, string>{
            {'X', "F[+X]F[-X]+X" },
            {'F', "FF" }
            };
            angle = 25.7f;
            length = 1;
            itinerations = 7;
            DestroyObjects();
            Generate();
        }
        else if (val == 7)
        {
            transformStack = new Stack<Trans>();
            rules = new Dictionary<char, string>{
            {'X', "F+[[X]-X]-F[-FX]+X" },
            {'F', "FF" }
            };
            angle = 25.7f;
            length = 1;
            itinerations = 7;
            DestroyObjects();
            Generate();
        }
        else if (val == 8)
        {
            transformStack = new Stack<Trans>();
            rules = new Dictionary<char, string>{
            {'X', "[FX][-FX][+FX]" },
            {'F', "FF" }
            };
            angle = 22.5f;
            length = 3;
            itinerations = 6;
            DestroyObjects();
            Generate();
        }
        else if (val == 9)
        {
            transformStack = new Stack<Trans>();
            rules = new Dictionary<char, string>{
            {'X', "[-F[X]-FX]+F[+FX]-X" },
            {'F', "FF" }
            };
            angle = 26.3f;
            length = 1;
            itinerations = 7;
            DestroyObjects();
            Generate();
        }
    }

    private void DestroyObjects()
    {
        GameObject[] branches = GameObject.FindGameObjectsWithTag("tree");
        foreach (GameObject b in branches)
        {
            Destroy(b);
        }
    }

    public void RandomAngle()
    {
        if (toggle.isOn)
        {
            angle = UnityEngine.Random.Range(randomvalue1, randomvalue2);
        }   
    }

    public void MinimumValue(string s)
    {
        randomvalue1 = float.Parse(s);
    }

    public void MaximumValue(string s)
    {
        randomvalue2 = float.Parse(s);
    }


    public void SetAngle(int val)
    {
        angle = angle + (5 * val) - preAngle;
        preAngle = (5 * val);
        Generate();
    }

    public void SetLenght(int val)
    {
        length = val + 1;
        Generate();
    }



}

