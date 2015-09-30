//-*- java -*-
using UnityEngine;

public class AttackManager : MonoBehaviour{

       public GameObject[] attacks;
       public string[] keyBindings; 

       [HideInInspector] 
       public GameObject activeAttack ;

       void Start(){
       activeAttack = attacks[0]; 
       for(int i = 1; i<attacks.Length; i++){
       attacks[i].SetActive(false); 
}
}

       void Update(){
       for(int i = 0; i<keyBindings.Length; i++){
       if(Input.GetKeyDown(keyBindings[i])){
	activeAttack.SetActive(false); 
	attacks[i].SetActive(true); 
	activeAttack = attacks[i]; 
}
}
}
       

}
