using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TeamManager
{
    [Serializable]
    public class Team : IEnumerable
    {
        private readonly List<GameObject> players;
        public readonly int Capacity;
        public int Points { get; set; }

        public Team(int teamSize)
        {
            Capacity = teamSize;
            players = new List<GameObject>(Capacity);
            Points = 0;
        }

        public int Count => players.Count;

        public bool IsEmpty => players.Count == 0;

        public bool IsFull => players.Count == Capacity;

        public GameObject Get(int index)
        {
            if (index > -1 && index < Capacity)
            {
                return players[index];
            }
            return null;
        }

        public bool AddPlayer(GameObject player)
        {
            if (players.Count < Capacity && player != null)
            {
                players.Add(player);
                return true;
            }
            return false;
        }

        public bool RemovePlayer(GameObject player)
        {
            return players.Remove(player);
        }

        public bool ReplacePlayer(GameObject oldPlayer, GameObject newPlayer)
        {
            if (newPlayer == null)
            {
                return false;
            }

            int slot = players.IndexOf(oldPlayer);
            return Replace(slot, newPlayer);
        }

        // A player can be only added to a full team if there is replaceable bot.
        public bool ReplaceBot(GameObject player)
        {
            if (player == null)
            {
                return false;
            }
            int slot = players.FindIndex(b => b.GetComponent<MenuCursor>() == null);
            return Replace(slot, player);
        }

        private bool Replace(int slot, GameObject player)
        {
            if (slot > -1)
            {
                players[slot] = player;
                return true;
            }
            return false;
        }

        public IEnumerator GetEnumerator()
        {
            return players.GetEnumerator();
        }

        public List<GameObject> FilterPlayers(Predicate<GameObject> predicate)
        {
            return players.FindAll(predicate);
        }

        public bool HasPlayer(GameObject player)
        {
            return players.Contains(player);
        }

        public bool ExistsPlayer(Predicate<GameObject> predicate)
        {
            return players.Exists(predicate);
        }
    }
}
