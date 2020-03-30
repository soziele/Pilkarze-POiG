using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_1C
{
    internal class Player
    {
       
        #region Prop
        public string Name { get; set; }
        public string Surname { get; set; }
        public uint Age { get; set; }
        public uint Weight { get; set; }
        #endregion

        #region constr
        public Player(string Name, string Surname, uint Age, uint Weight)
        {
           this.Name = Name;
           this.Surname = Surname;
           this.Age = Age;
           this.Weight = Weight;
        }

        #endregion

        #region methods

        //sprawdza czy obiekt ma ten sam stan co bieżąca instancja
        public bool isTheSame(Player player)
        {
            if (player.Surname != Surname) return false;
            if (player.Name != Name) return false;
            if (player.Age != Age) return false;
            if (player.Weight != Weight) return false;
            return true;
        }

        public override string ToString()
        {
            return $"{Surname} {Name} lat: {Age} waga: {Weight} kg";
        }

        public string ToFileFormat()
        {
            return $"{Surname}|{Name}|{Age}|{Weight}";
        }

        public static Player CreateFromString(string sPilkarz)
        {
            string Name, Surname;
            uint Age, Weight;
            var pola = sPilkarz.Split('|');
            if(pola.Length==4)
            {
                Surname = pola[1];
                Name = pola[0];
                Age = uint.Parse(pola[2]);
                Weight = uint.Parse(pola[3]);
                return new Player(Name, Surname, Age, Weight);
            }
            throw new Exception("Błędny format danych z pliku");
        }
        #endregion
    }
}
