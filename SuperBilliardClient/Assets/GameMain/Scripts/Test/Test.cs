using UnityEngine;
using System.Xml.Linq;

namespace SuperBilliard
{
    public class Test : MonoBehaviour
    {
        private SnokkerBilliardLogic[] billiards;
        private string path = "E:\\Program Files (x86)\\gameProject\\SuperBilliard\\DataTableGenerator\\Snokker.xml";
        public void Start()
        {
            //billiards = GetComponentsInChildren<SnokkerBilliardLogic>();
            //XDocument doc = new XDocument();
            //XElement root = new XElement("Billiards");
            //doc.Add(root);
            //foreach (var item in billiards)
            //{
            //    root.Add(new XElement("FancyBilliard", new XAttribute("ID", item.BilliardDDId), new XAttribute("Position", item.transform.position.ToString()), new XAttribute("EulerAngle", item.transform.rotation.eulerAngles.ToString())));
            //}
            //doc.Save(path);
        }
    }
}