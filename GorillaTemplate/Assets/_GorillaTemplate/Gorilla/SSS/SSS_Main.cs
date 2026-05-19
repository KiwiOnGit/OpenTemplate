using UnityEngine;
using GorillaLocomotion;
using Normal.Realtime;

namespace SnowSecuredServers
{
    public class SSS_Main : MonoBehaviour
    
    {

        private static int _obfuscatedValue = 482910; //will look like random nothing to cheaters/memory hackers
        private static int _secretXorKey = 57291;
        public Player _Player;

        public Realtime _Realtime;
        protected float _maxArmLength;

        protected float _jumpMultiplier;

        protected bool Detected;

        void Start()
        {
            InitSnowProtectedServers(); //moved from start to its own function for, well I dont know its protected now so thats good.
        }

        void Update()
        {
            CheckLocalPlayer();

            if (Detected) //if cheating we will NOT allow them back onto the servers until they have disabled cheats. i'll add automatic playfab banning and logging to something like a discord channel.
            {
                _Realtime.Disconnect(); //spam disconnect, they will not join any new lobbies until their cheats are gone!
                Debug.Log("Cheater! Cheater! Cheater!");
            }
        }

        protected void InitSnowProtectedServers()
        {
            _secretXorKey = Random.Range(10000, 99999); // Xor gate key gets randomized
            _obfuscatedValue = 1 ^ _secretXorKey; 


            if (_Player == null){_Player = Object.FindAnyObjectByType<Player>();}
            if (_Realtime == null){_Realtime = Object.FindAnyObjectByType<Realtime>();}


            _maxArmLength = _Player.maxArmLength;
            _jumpMultiplier = _Player.jumpMultiplier;
        }

        public static bool IsSnowSecuredServersActive
        {
            get
            {
               int decrypted = _obfuscatedValue ^ _secretXorKey;
               return decrypted == 1; 
            }
            
        }


        internal void CheckLocalPlayer() //Checks the local player for modifications.
        {
            if (_Player.maxArmLength != _maxArmLength)
            {
                Detected = true;
            }

            if (_jumpMultiplier != _Player.jumpMultiplier)
            {
                Detected = true;
            }
        }
    }


}