using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEditor.PackageManager;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Assets.AdditiveAssets.PolygonFantasyHeroCharacters.Scripts;

namespace PsychoticLab
{
    [Serializable]
    public enum Gender { Male, Female }
    [Serializable]
    public enum Race { Human, Elf }
    public enum SkinColor { White, Brown, Black, Elf }
    public enum Elements { Yes, No }
    public enum HeadCovering { HeadCoverings_Base_Hair, HeadCoverings_No_FacialHair, HeadCoverings_No_Hair }
    public enum FacialHair { Yes, No }
    public enum ActForItem { Add, Remove, None }
    [Serializable]
    public enum BodyItems
    {
        HeadAllElements, HeadNoElements, Ear, EyeBrow, Hair, FacialHair, Torso, ArmUpper_Right, ArmUpper_Left, ArmLower_Right, ArmLower_Left, Hand_Right, Hand_Left, Hips, Leg_Right, Leg_Left,
        Extra, Head_Attachment, Back_Attachment, Chest_Attachment, Elbow_Attachment_Left, Elbow_Attachment_Right, HeadCoverings_Base_Hair, HeadCoverings_No_FacialHair,
        HeadCoverings_No_Hair, Hips_Attachment, Knee_Attachement_Left, Knee_Attachement_Right, Shoulder_Attachment_Left, Shoulder_Attachment_Right,
    }

    [Serializable]
    public class Item
    {
        public GameObject Object;
        public int Position;
        public bool Visible;

        public Item() { }

        public Item(GameObject _obj, int _position, bool _visible)
        {
            Object = _obj;
            Position = _position;
            Visible = _visible;
        }

        public void Clear()
        {
            Object = null;
            Position = 0;
        }
    }

    [Serializable]
    public class SaveItem
    {
        public BodyItems Body;
        public int Position;
        public bool Visible;

        public SaveItem()
        {

        }

        public SaveItem(BodyItems _body, Item _item)
        {
            Body = _body;
            Position = _item.Position;
            Visible = _item.Visible;
        }
    }

    [Serializable]
    public class SaveCharacter
    {
        public Race Race;
        public Gender Gender;
        public List<bool> Toggle = new List<bool>();
        public List<SaveItem> Items = new List<SaveItem>();

        public SaveCharacter()
        {

        }

        public SaveCharacter(List<bool> _toggle, List<SaveItem> _item, Gender _gender, Race _race)
        {
            Toggle = _toggle;
            Items = _item;
            Gender = _gender;
            Race = _race;
        }
    }


    public class Character
    {
        public Race Race;
        public Gender Gander;
        public Dictionary<BodyItems, Item> Body = new Dictionary<BodyItems, Item>();
        private readonly CharacterObjectGroups _male;
        private readonly CharacterObjectGroups _female;
        private readonly CharacterObjectListsAllGender _allGender;
        public readonly Dictionary<Gender, Dictionary<BodyItems, List<GameObject>>> GenderDict;
        public readonly Dictionary<BodyItems, List<GameObject>> NoGenderDict;

        public bool HairToggle = true;
        public bool FacialHairToggle = true;
        public bool HeadAllElementsToggle = true;
        public bool HeadCov1Toggle = false;
        public bool HeadCov2Toggle = false;
        public bool HeadCov3Toggle = false;
        public bool ChestToggle = false;
        public bool SholderRToggle = false;
        public bool SholderLToggle = false;
        public bool ElbowRToggle = false;
        public bool ElbowLToggle = false;
        public bool KneeRToggle = false;
        public bool KneeLToggle = false;
        public bool HipsAToggle = false;

        public readonly HashSet<BodyItems> GenderHash;

        public Character(CharacterObjectGroups male, CharacterObjectGroups female, CharacterObjectListsAllGender allGender)
        {
            _male = male;
            _female = female;
            _allGender = allGender;

            GenderHash = new HashSet<BodyItems> 
            { 
                BodyItems.HeadAllElements, BodyItems.HeadNoElements, BodyItems.EyeBrow, BodyItems.FacialHair, BodyItems.Torso,
                BodyItems.ArmUpper_Right, BodyItems.ArmUpper_Left, BodyItems.ArmLower_Right, BodyItems.ArmLower_Left,
                BodyItems.Hand_Right, BodyItems.Hand_Left, BodyItems.Hips, BodyItems.Leg_Right, BodyItems.Leg_Left 
            };

            Race = Race.Human;
            GenderDict = new Dictionary<Gender, Dictionary<BodyItems, List<GameObject>>>
            {
                {Gender.Male,  new Dictionary<BodyItems, List<GameObject>>
                {
                    { BodyItems.ArmLower_Left, male.arm_Lower_Left },
                    { BodyItems.ArmLower_Right, male.arm_Lower_Right },
                    { BodyItems.ArmUpper_Left, male.arm_Upper_Left },
                    { BodyItems.ArmUpper_Right, male.arm_Upper_Right },
                    { BodyItems.EyeBrow, male.eyebrow },
                    { BodyItems.FacialHair, male.facialHair },
                    { BodyItems.Hand_Left, male.hand_Left },
                    { BodyItems.Hand_Right, male.hand_Right },
                    { BodyItems.HeadAllElements, male.headAllElements },
                    { BodyItems.HeadNoElements, male.headNoElements },
                    { BodyItems.Hips, male.hips },
                    { BodyItems.Leg_Left, male.leg_Left },
                    { BodyItems.Leg_Right, male.leg_Right },
                    { BodyItems.Torso, male.torso },
                }},
                
                {Gender.Female,  new Dictionary<BodyItems, List<GameObject>>
                {
                    { BodyItems.ArmLower_Left, female.arm_Lower_Left },
                    { BodyItems.ArmLower_Right, female.arm_Lower_Right },
                    { BodyItems.ArmUpper_Left, female.arm_Upper_Left },
                    { BodyItems.ArmUpper_Right, female.arm_Upper_Right },
                    { BodyItems.EyeBrow, female.eyebrow },
                    { BodyItems.FacialHair, female.facialHair },
                    { BodyItems.Hand_Left, female.hand_Left },
                    { BodyItems.Hand_Right, female.hand_Right },
                    { BodyItems.HeadAllElements, female.headAllElements },
                    { BodyItems.HeadNoElements, female.headNoElements },
                    { BodyItems.Hips, female.hips },
                    { BodyItems.Leg_Left, female.leg_Left },
                    { BodyItems.Leg_Right, female.leg_Right },
                    { BodyItems.Torso, female.torso },
                }},
            };

            NoGenderDict = new Dictionary<BodyItems, List<GameObject>>
            {
                {BodyItems.Extra,  _allGender.all_12_Extra},
                {BodyItems.Hair,  _allGender.all_Hair},
                {BodyItems.Head_Attachment,  _allGender.all_Head_Attachment},
                {BodyItems.Back_Attachment,  _allGender.back_Attachment},
                {BodyItems.Chest_Attachment,  _allGender.chest_Attachment},
                {BodyItems.Elbow_Attachment_Left,  _allGender.elbow_Attachment_Left},
                {BodyItems.Elbow_Attachment_Right,  _allGender.elbow_Attachment_Right},
                {BodyItems.Ear,  _allGender.elf_Ear},
                {BodyItems.HeadCoverings_Base_Hair,  _allGender.headCoverings_Base_Hair},
                {BodyItems.HeadCoverings_No_FacialHair,  _allGender.headCoverings_No_FacialHair},
                {BodyItems.HeadCoverings_No_Hair,  _allGender.headCoverings_No_Hair},
                {BodyItems.Hips_Attachment,  _allGender.hips_Attachment},
                {BodyItems.Knee_Attachement_Left,  _allGender.knee_Attachement_Left},
                {BodyItems.Knee_Attachement_Right,  _allGender.knee_Attachement_Right},
                {BodyItems.Shoulder_Attachment_Left,  _allGender.shoulder_Attachment_Left},
                {BodyItems.Shoulder_Attachment_Right,  _allGender.shoulder_Attachment_Right},
            };

            Body.Add(BodyItems.HeadAllElements, new Item(_male.headAllElements.First(), 0, true));
            Body.Add(BodyItems.HeadNoElements, new Item(_male.headNoElements.First(), 0, false));
            Body.Add(BodyItems.FacialHair, new Item(_male.facialHair.First(), 0, true));
            Body.Add(BodyItems.Hair, new Item(_allGender.all_Hair.First(), 0, true));
            Body.Add(BodyItems.Ear, new Item(_allGender.elf_Ear.First(), 0, false));
            Body.Add(BodyItems.EyeBrow, new Item(_male.eyebrow.First(), 0, true));
            Body.Add(BodyItems.Torso, new Item(_male.torso.First(), 0, true));
            Body.Add(BodyItems.ArmUpper_Right, new Item(_male.arm_Upper_Right.First(), 0, true));
            Body.Add(BodyItems.ArmUpper_Left, new Item(_male.arm_Upper_Left.First(), 0, true));
            Body.Add(BodyItems.ArmLower_Right, new Item(_male.arm_Lower_Right.First(), 0, true));
            Body.Add(BodyItems.ArmLower_Left, new Item(_male.arm_Lower_Left.First(), 0, true));
            Body.Add(BodyItems.Hand_Right, new Item(_male.hand_Right.First(), 0, true));
            Body.Add(BodyItems.Hand_Left, new Item(_male.hand_Left.First(), 0, true));
            Body.Add(BodyItems.Hips, new Item(_male.hips.First(), 0, true));
            Body.Add(BodyItems.Leg_Right, new Item(_male.leg_Right.First(), 0, true));
            Body.Add(BodyItems.Leg_Left, new Item(_male.leg_Left.First(), 0, true));
                
            Body.Add(BodyItems.Head_Attachment, new Item(NoGenderDict[BodyItems.Head_Attachment].First(), 0, false));
            Body.Add(BodyItems.Back_Attachment, new Item(NoGenderDict[BodyItems.Back_Attachment].First(), 0, false));
            //Body.Add(BodyItems.Chest_Attachment, new Item(_noGenderDict[BodyItems.Chest_Attachment].First(), 0));
            Body.Add(BodyItems.Elbow_Attachment_Left, new Item(NoGenderDict[BodyItems.Elbow_Attachment_Left].First(), 0, false));
            Body.Add(BodyItems.Elbow_Attachment_Right, new Item(NoGenderDict[BodyItems.Elbow_Attachment_Right].First(), 0, false));
            Body.Add(BodyItems.HeadCoverings_Base_Hair, new Item(NoGenderDict[BodyItems.HeadCoverings_Base_Hair].First(), 0, false));
            Body.Add(BodyItems.HeadCoverings_No_FacialHair, new Item(NoGenderDict[BodyItems.HeadCoverings_No_FacialHair].First(), 0, false));
            Body.Add(BodyItems.HeadCoverings_No_Hair, new Item(NoGenderDict[BodyItems.HeadCoverings_No_Hair].First(), 0, false));
            Body.Add(BodyItems.Hips_Attachment, new Item(NoGenderDict[BodyItems.Hips_Attachment].First(), 0, false));
            Body.Add(BodyItems.Knee_Attachement_Left, new Item(NoGenderDict[BodyItems.Knee_Attachement_Left].First(), 0, false));
            Body.Add(BodyItems.Knee_Attachement_Right, new Item(NoGenderDict[BodyItems.Knee_Attachement_Right].First(), 0, false));
            Body.Add(BodyItems.Shoulder_Attachment_Left, new Item(NoGenderDict[BodyItems.Shoulder_Attachment_Left].First(), 0, false));
            Body.Add(BodyItems.Shoulder_Attachment_Right, new Item(NoGenderDict[BodyItems.Shoulder_Attachment_Right].First(), 0, false));
        }

        public List<Tuple<ActForItem, List<Item>>> ReadAllCharacter()
        {
            var list = Body.Select((x, y) => x.Value).ToList();
            var listrRemove = new List<Item>();
            if (!HairToggle)
            {
                list.Remove(Body[BodyItems.Hair]);
                listrRemove.Add(Body[BodyItems.Hair]);
            }

            if(!FacialHairToggle)
            {
                list.Remove(Body[BodyItems.FacialHair]);
                listrRemove.Add(Body[BodyItems.FacialHair]);
            }

            if (!HeadAllElementsToggle)
            {
                list.Remove(Body[BodyItems.HeadNoElements]);
            }
            else
            {
                list.Remove(Body[BodyItems.HeadAllElements]);
            }

            if (!HeadCov1Toggle)
            {
                list.Remove(Body[BodyItems.HeadCoverings_Base_Hair]);
                list.Remove(Body[BodyItems.HeadCoverings_No_Hair]);
                list.Remove(Body[BodyItems.HeadCoverings_No_FacialHair]);
            }

            if (!HeadCov2Toggle)
            {
                list.Remove(Body[BodyItems.HeadCoverings_Base_Hair]);
                list.Remove(Body[BodyItems.HeadCoverings_No_Hair]);
                list.Remove(Body[BodyItems.HeadCoverings_No_FacialHair]);
            }

            if (!HeadCov3Toggle)
            {
                list.Remove(Body[BodyItems.HeadCoverings_Base_Hair]);
                list.Remove(Body[BodyItems.HeadCoverings_No_Hair]);
                list.Remove(Body[BodyItems.HeadCoverings_No_FacialHair]);
            }

            list.Remove(Body[BodyItems.HeadCoverings_Base_Hair]);
            list.Remove(Body[BodyItems.HeadCoverings_No_Hair]);
            list.Remove(Body[BodyItems.HeadCoverings_No_FacialHair]);

            //if (!ChestToggle)
            //{
            //    list.Remove(Body[BodyItems.Chest_Attachment]);
            //}

            if (!SholderRToggle)
            {
                list.Remove(Body[BodyItems.Shoulder_Attachment_Right]);
            }

            if (!SholderLToggle)
            {
                list.Remove(Body[BodyItems.Shoulder_Attachment_Left]);
            }

            if (!ElbowRToggle)
            {
                list.Remove(Body[BodyItems.Elbow_Attachment_Right]);
            }

            if (!ElbowLToggle)
            {
                list.Remove(Body[BodyItems.Elbow_Attachment_Left]);
            }

            if (!KneeRToggle)
            {
                list.Remove(Body[BodyItems.Knee_Attachement_Right]);
            }

            if (!KneeLToggle)
            {
                list.Remove(Body[BodyItems.Knee_Attachement_Left]);
            }

            if (!HipsAToggle)
            {
                list.Remove(Body[BodyItems.Hips_Attachment]);
            }

            return new List<Tuple<ActForItem, List<Item>>> { Tuple.Create(ActForItem.Add, Body.Select((x, y) => x.Value).ToList()),
            Tuple.Create(ActForItem.Remove, listrRemove )};
        }

        public List<Tuple<ActForItem, List<Item>>> ClearAllCharacter()
        {
            var list = Body.Select((x, y) => x.Value).ToList();
            list.ForEach(x => new Item( x.Object, x.Position, false));
            return new List<Tuple<ActForItem, List<Item>>> { Tuple.Create(ActForItem.Remove, list) };
        }

        public int GetPosition(BodyItems body)
        {
            return Body[body].Position;
        }

        public List<Tuple<ActForItem, List<Item>>> ChangeRace(Race _race)
        {
            Race = _race;
            if (_race == Race.Elf)
            {
                Body[BodyItems.Ear].Object = _allGender.elf_Ear[Body[BodyItems.Ear].Position];
                Body[BodyItems.Ear].Visible = true;
                return new List<Tuple<ActForItem, List<Item>>> { Tuple.Create(ActForItem.Add, new List<Item> { Body[BodyItems.Ear] }) };
            }
            else if (_race == Race.Human)
            {
                var ear = Body[BodyItems.Ear].Object;
                Body[BodyItems.Ear].Object = _allGender.elf_Ear[Body[BodyItems.Ear].Position];
                Body[BodyItems.Ear].Visible = false;
                return new List<Tuple<ActForItem, List<Item>>> { Tuple.Create(ActForItem.Remove, new List<Item> { new Item(ear, Body[BodyItems.Ear].Position, false) }) };
            }
            else
            {
                return new List<Tuple<ActForItem, List<Item>>> { Tuple.Create(ActForItem.None, new List<Item> { }) };
            }
        }

        public List<Tuple<ActForItem, List<Item>>> ChangeGender(Gender _gender)
        {
            if (_gender == Gander)
            {
                return new List<Tuple<ActForItem, List<Item>>> { Tuple.Create(ActForItem.None, new List<Item> { }) };
            }

            Gander = _gender;

            var list = new List<Tuple<ActForItem, List<Item>>> { };

            list.Add(Tuple.Create(ActForItem.Remove, new List<Item>
                    {
                        Body[BodyItems.HeadAllElements],
                        Body[BodyItems.EyeBrow],
                        Body[BodyItems.FacialHair],
                        Body[BodyItems.Torso],
                        Body[BodyItems.ArmUpper_Right],
                        Body[BodyItems.ArmUpper_Left],
                        Body[BodyItems.ArmLower_Right],
                        Body[BodyItems.ArmLower_Left],
                        Body[BodyItems.Hand_Right],
                        Body[BodyItems.Hand_Left],
                        Body[BodyItems.Hips],
                        Body[BodyItems.Leg_Right],
                        Body[BodyItems.Leg_Left],
                    }));

            Body[BodyItems.HeadAllElements] = ItemGenderChange(Body[BodyItems.HeadAllElements], _gender, _male.headAllElements, _female.headAllElements);
            Body[BodyItems.EyeBrow] = ItemGenderChange(Body[BodyItems.EyeBrow], _gender, _male.eyebrow, _female.eyebrow);
            Body[BodyItems.FacialHair] = ItemGenderChange(Body[BodyItems.Hair], _gender, _male.facialHair, _female.facialHair);
            Body[BodyItems.Torso] = ItemGenderChange(Body[BodyItems.Torso], _gender, _male.torso, _female.torso);
            Body[BodyItems.ArmUpper_Right] = ItemGenderChange(Body[BodyItems.ArmUpper_Right], _gender, _male.arm_Upper_Right, _female.arm_Upper_Right);
            Body[BodyItems.ArmUpper_Left] = ItemGenderChange(Body[BodyItems.ArmUpper_Left], _gender, _male.arm_Upper_Left, _female.arm_Upper_Left);
            Body[BodyItems.ArmLower_Right] = ItemGenderChange(Body[BodyItems.ArmLower_Right], _gender, _male.arm_Lower_Right, _female.arm_Lower_Right);
            Body[BodyItems.ArmLower_Left] = ItemGenderChange(Body[BodyItems.ArmLower_Left], _gender, _male.arm_Lower_Left, _female.arm_Lower_Left);
            Body[BodyItems.Hand_Right] = ItemGenderChange(Body[BodyItems.Hand_Right], _gender, _male.hand_Right, _female.hand_Right);
            Body[BodyItems.Hand_Left] = ItemGenderChange(Body[BodyItems.Hand_Left], _gender, _male.hand_Left, _female.hand_Left);
            Body[BodyItems.Hips] = ItemGenderChange(Body[BodyItems.Hips], _gender, _male.hips, _female.hips);
            Body[BodyItems.Leg_Right] = ItemGenderChange(Body[BodyItems.Leg_Right], _gender, _male.leg_Right, _female.leg_Right);
            Body[BodyItems.Leg_Left] = ItemGenderChange(Body[BodyItems.Leg_Left], _gender, _male.leg_Left, _female.leg_Left);

            list.Add(Tuple.Create(ActForItem.Add, new List<Item>
                    {
                        Body[BodyItems.HeadAllElements],
                        Body[BodyItems.EyeBrow],
                        Body[BodyItems.FacialHair],
                        Body[BodyItems.Torso],
                        Body[BodyItems.ArmUpper_Right],
                        Body[BodyItems.ArmUpper_Left],
                        Body[BodyItems.ArmLower_Right],
                        Body[BodyItems.ArmLower_Left],
                        Body[BodyItems.Hand_Right],
                        Body[BodyItems.Hand_Left],
                        Body[BodyItems.Hips],
                        Body[BodyItems.Leg_Right],
                        Body[BodyItems.Leg_Left],
                    }));

            return list;
        }

        public Item ItemGenderChange(Item _item, Gender _gender, List<GameObject> _list_male, List<GameObject> _list_female)
        {
            if ((_gender is Gender.Male
                ? _list_male.Count
                : _list_female.Count) <= _item.Position || _item.Position < 0)
            {
                _item.Position = (_gender is Gender.Male
                    ? _list_male.Count
                    : _list_female.Count) - 1;
            }

            return new Item (_item.Position < 0 
                ? null 
                    : _gender is Gender.Male
                ? _list_male[_item.Position]
                    : _list_female[_item.Position], _item.Position,
                    _item.Visible);
        }

        public List<Tuple<ActForItem, List<Item>>> ChangeForIndex(BodyItems body, int index)
        {
            //if (!Body.TryGetValue(body, out var _item) || _item.Position == index)
            //{
            //    return new List<Tuple<ActForItem, List<Item>>> { Tuple.Create(ActForItem.None, new List<Item> { }) };
            //}

            var list = new List<Tuple<ActForItem, List<Item>>> { Tuple.Create(ActForItem.Remove, new List<Item> { new Item(Body[body].Object, Body[body].Position, false) }) };

            if (GenderHash.Contains(body))
            {
                if (GenderDict[Gander][body].Count <= index)
                {
                    index = GenderDict[Gander][body].Count - 1;
                }

                Body[body].Object = GenderDict[Gander][body][index];
            }
            else
            {
                if (NoGenderDict[body].Count <= index)
                {
                    index = NoGenderDict[body].Count - 1;
                }

                Body[body].Object = NoGenderDict[body][index];
            }
            Body[body].Position = index;
            Body[body].Visible = true;

            list.Add(Tuple.Create(ActForItem.Add, new List<Item> { Body[body] })); 
            return list;
        }

        public List<Tuple<ActForItem, List<Item>>> ChangeNext(BodyItems body)
        {
            if (!Body.TryGetValue(body, out var _))
            {
                return new List<Tuple<ActForItem, List<Item>>> { Tuple.Create(ActForItem.None, new List<Item> { }) };
            }

            var list = new List<Tuple<ActForItem, List<Item>>> { Tuple.Create(ActForItem.Remove, new List<Item> { new Item(Body[body].Object, Body[body].Position, false) }) };

            if (GenderHash.Contains(body))
            {
                Body[body].Position++;
                Body[body].Position = GenderDict[Gander][body].Count <= Body[body].Position ? 0 : Body[body].Position;
                Body[body].Object = GenderDict[Gander][body][Body[body].Position];
                Body[body].Visible = true;
            }
            else
            {
                Body[body].Position++;
                Body[body].Position = NoGenderDict[body].Count <= Body[body].Position ? 0 : Body[body].Position;
                Body[body].Object = NoGenderDict[body][Body[body].Position];
                Body[body].Visible = true;
            }

            list.Add(Tuple.Create(ActForItem.Add, new List<Item> { Body[body] }));
            return list;
        }

        public List<Tuple<ActForItem, List<Item>>> ChangePrevious(BodyItems body)
        {
            if (!Body.TryGetValue(body, out var _item))
            {
                return new List<Tuple<ActForItem, List<Item>>> { Tuple.Create(ActForItem.None, new List<Item> { }) };
            }

            var list = new List<Tuple<ActForItem, List<Item>>> { Tuple.Create(ActForItem.Remove, new List<Item> { new Item(Body[body].Object, Body[body].Position, false) }) };

            if (GenderHash.Contains(body))
            {
                Body[body].Position--;
                Body[body].Position = 0 > Body[body].Position ? GenderDict[Gander][body].Count - 1 : Body[body].Position;
                Body[body].Object = GenderDict[Gander][body][Body[body].Position];
                Body[body].Visible = true;
            }
            else
            {
                Body[body].Position--;
                Body[body].Position = 0 > Body[body].Position ? NoGenderDict[body].Count - 1 : Body[body].Position;
                Body[body].Object = NoGenderDict[body][Body[body].Position];
                Body[body].Visible = true;
            }

            list.Add(Tuple.Create(ActForItem.Add, new List<Item> { Body[body] }));
            return list;
        }

        public List<Tuple<ActForItem, List<Item>>> ChangeDisable(BodyItems body)
        {
            if (!Body.TryGetValue(body, out var _))
            {
                return new List<Tuple<ActForItem, List<Item>>> { Tuple.Create(ActForItem.None, new List<Item> { }) };
            }

            return new List<Tuple<ActForItem, List<Item>>> { Tuple.Create(ActForItem.Remove, new List<Item> { new Item(Body[body].Object, Body[body].Position, false) }) };
        }

        public List<Tuple<ActForItem, List<Item>>> ChangeEnable(BodyItems body)
        {
            if (!Body.TryGetValue(body, out var _))
            {
                return new List<Tuple<ActForItem, List<Item>>> { Tuple.Create(ActForItem.None, new List<Item> { }) };
            }

            return new List<Tuple<ActForItem, List<Item>>> { Tuple.Create(ActForItem.Add, new List<Item> { new Item(Body[body].Object, Body[body].Position, true) }) };
        }

        internal void Load(SaveCharacter date)
        {
            if (date.Toggle.Count >= 14)
            {
                HairToggle = date.Toggle[0];
                FacialHairToggle = date.Toggle[1];
                HeadAllElementsToggle = date.Toggle[2];
                HeadCov1Toggle = date.Toggle[3];
                HeadCov2Toggle = date.Toggle[4];
                HeadCov3Toggle = date.Toggle[5];
                ChestToggle = date.Toggle[6];
                SholderRToggle = date.Toggle[7];
                SholderLToggle = date.Toggle[8];
                ElbowRToggle = date.Toggle[9];
                ElbowLToggle = date.Toggle[10];
                KneeRToggle = date.Toggle[11];
                KneeLToggle = date.Toggle[12];
                HipsAToggle = date.Toggle[13];
            }

            Gander = date.Gender;
            Race = date.Race;

            foreach(var item in date.Items)
            {
                if ((GenderHash.Contains(item.Body) ? GenderDict[date.Gender][item.Body].Count : NoGenderDict[item.Body].Count) > item.Position)
                {
                    Body[item.Body].Object = GenderHash.Contains(item.Body) ? GenderDict[date.Gender][item.Body][item.Position] : NoGenderDict[item.Body][item.Position];
                    Body[item.Body].Position = item.Position;
                    Body[item.Body].Visible = item.Visible;
                }
            }
        }
    }


    public class CharacterRandomizer : MonoBehaviour
    {
        private bool Randomizer = false;
        public Stack<Character> MemoryCharacters = new Stack<Character>();
        public Stack<Character> PrevCharacters = new Stack<Character>();
        public Stack<Character> NextCharacters = new Stack<Character>();
        public Character Character;

        public string NameFile;


        [Header("Demo Settings")]
        public bool repeatOnPlay = false;
        public float shuffleSpeed = 0.7f;

        [Header("Material")]
        public Material mat;

        [Header("Gear Colors")]
        public Color[] primary = { new Color(0.2862745f, 0.4f, 0.4941177f), new Color(0.4392157f, 0.1960784f, 0.172549f), new Color(0.3529412f, 0.3803922f, 0.2705882f), new Color(0.682353f, 0.4392157f, 0.2196079f), new Color(0.4313726f, 0.2313726f, 0.2705882f), new Color(0.5921569f, 0.4941177f, 0.2588235f), new Color(0.482353f, 0.4156863f, 0.3529412f), new Color(0.2352941f, 0.2352941f, 0.2352941f), new Color(0.2313726f, 0.4313726f, 0.4156863f) };
        public Color[] secondary = { new Color(0.7019608f, 0.6235294f, 0.4666667f), new Color(0.7372549f, 0.7372549f, 0.7372549f), new Color(0.1647059f, 0.1647059f, 0.1647059f), new Color(0.2392157f, 0.2509804f, 0.1882353f) };

        [Header("Metal Colors")]
        public Color[] metalPrimary = { new Color(0.6705883f, 0.6705883f, 0.6705883f), new Color(0.5568628f, 0.5960785f, 0.6392157f), new Color(0.5568628f, 0.6235294f, 0.6f), new Color(0.6313726f, 0.6196079f, 0.5568628f), new Color(0.6980392f, 0.6509804f, 0.6196079f) };
        public Color[] metalSecondary = { new Color(0.3921569f, 0.4039216f, 0.4117647f), new Color(0.4784314f, 0.5176471f, 0.5450981f), new Color(0.3764706f, 0.3607843f, 0.3372549f), new Color(0.3254902f, 0.3764706f, 0.3372549f), new Color(0.4f, 0.4039216f, 0.3568628f) };

        [Header("Leather Colors")]
        public Color[] leatherPrimary;
        public Color[] leatherSecondary;

        [Header("Skin Colors")]
        public Color[] whiteSkin = { new Color(1f, 0.8000001f, 0.682353f) };
        public Color[] brownSkin = { new Color(0.8196079f, 0.6352941f, 0.4588236f) };
        public Color[] blackSkin = { new Color(0.5647059f, 0.4078432f, 0.3137255f) };
        public Color[] elfSkin = { new Color(0.9607844f, 0.7843138f, 0.7294118f) };

        [Header("Hair Colors")]
        public Color[] whiteHair = { new Color(0.3098039f, 0.254902f, 0.1764706f), new Color(0.2196079f, 0.2196079f, 0.2196079f), new Color(0.8313726f, 0.6235294f, 0.3607843f), new Color(0.8901961f, 0.7803922f, 0.5490196f), new Color(0.8000001f, 0.8196079f, 0.8078432f), new Color(0.6862745f, 0.4f, 0.2352941f), new Color(0.5450981f, 0.427451f, 0.2156863f), new Color(0.8470589f, 0.4666667f, 0.2470588f) };
        public Color whiteStubble = new Color(0.8039216f, 0.7019608f, 0.6313726f);
        public Color[] brownHair = { new Color(0.3098039f, 0.254902f, 0.1764706f), new Color(0.1764706f, 0.1686275f, 0.1686275f), new Color(0.3843138f, 0.2352941f, 0.0509804f), new Color(0.6196079f, 0.6196079f, 0.6196079f), new Color(0.6196079f, 0.6196079f, 0.6196079f) };
        public Color brownStubble = new Color(0.6588235f, 0.572549f, 0.4627451f);
        public Color[] blackHair = { new Color(0.2431373f, 0.2039216f, 0.145098f), new Color(0.1764706f, 0.1686275f, 0.1686275f), new Color(0.1764706f, 0.1686275f, 0.1686275f) };
        public Color blackStubble = new Color(0.3882353f, 0.2901961f, 0.2470588f);
        public Color[] elfHair = { new Color(0.9764706f, 0.9686275f, 0.9568628f), new Color(0.1764706f, 0.1686275f, 0.1686275f), new Color(0.8980393f, 0.7764707f, 0.6196079f) };
        public Color elfStubble = new Color(0.8627452f, 0.7294118f, 0.6862745f);

        [Header("Scar Colors")]
        public Color whiteScar = new Color(0.9294118f, 0.6862745f, 0.5921569f);
        public Color brownScar = new Color(0.6980392f, 0.5450981f, 0.4f);
        public Color blackScar = new Color(0.4235294f, 0.3176471f, 0.282353f);
        public Color elfScar = new Color(0.8745099f, 0.6588235f, 0.6313726f);

        [Header("Body Art Colors")]
        public Color[] bodyArt = { new Color(0.0509804f, 0.6745098f, 0.9843138f), new Color(0.7215686f, 0.2666667f, 0.2666667f), new Color(0.3058824f, 0.7215686f, 0.6862745f), new Color(0.9254903f, 0.882353f, 0.8509805f), new Color(0.3098039f, 0.7058824f, 0.3137255f), new Color(0.5294118f, 0.3098039f, 0.6470588f), new Color(0.8666667f, 0.7764707f, 0.254902f), new Color(0.2392157f, 0.4588236f, 0.8156863f) };

        // list of enabed objects on character
        [HideInInspector]
        public List<GameObject> enabledObjects = new List<GameObject>();

        // character object lists
        // male list
        [HideInInspector]
        public CharacterObjectGroups male;

        // female list
        [HideInInspector]
        public CharacterObjectGroups female;

        // universal list
        [HideInInspector]
        public CharacterObjectListsAllGender allGender;

        // reference to camera transform, used for rotation around the model during or after a randomization (this is sourced from Camera.main, so the main camera must be in the scene for this to work)
        Transform camHolder;

        // cam rotation x
        float x = 16;

        // cam rotation y
        float y = -30;


        public void SaveFile()
        {
            SaveLoadDialog.SaveCharacter(
                new SaveCharacter(
                    new List<bool> { Character.HairToggle, Character.FacialHairToggle, Character.HeadAllElementsToggle, Character.HeadCov1Toggle, Character.HeadCov2Toggle, Character.HeadCov3Toggle, Character.ChestToggle, Character.SholderRToggle, Character.SholderLToggle, Character.ElbowRToggle, Character.ElbowLToggle, Character.KneeRToggle, Character.KneeLToggle, Character.HipsAToggle },
                    Character.Body.Select((x, y) => new SaveItem(x.Key, new Item(null, x.Value.Position, x.Value.Visible))).ToList(),
                    Character.Gander,
                    Character.Race),
                NameFile);            
        }

        public void NewFileName(string _fileName)
        {
            NameFile = _fileName;
        }

        public void LoadFile()
        {
            var _date = SaveLoadDialog.LoadCharacter(NameFile);
            if (_date is null) return;
            PrevCharacters.Push(Character);
            ActivateItem(Character.ClearAllCharacter());
            Character = new Character(male, female, allGender);
            Character.Load(_date);
            ActivateItem(Character.ReadAllCharacter());
        }

        public void RandomazerToggle(bool value)
        {
            Randomizer = value;
        }

        public void ElfChange()
        {            
            ElfToggle(Character.Race == Race.Human ? true : false);
        }

        public void ElfToggle(bool value)
        {
            if (!Character.HeadAllElementsToggle)
            {
                ActivateItem(Character.ChangeRace(value ? Race.Elf : Race.Human));
            }
            else
            {
                if (Character.Body[BodyItems.HeadNoElements].Position == 0)
                {
                    ActivateItem(Character.ChangeRace(value ? Race.Elf : Race.Human));
                }
                else
                {
                    Character.Race = value ? Race.Elf : Race.Human;
                }

            }
        }

        public void GenderChange(int pos)
        {
            ActivateItem(Character.ChangeGender(pos == 0 ? Gender.Male : Gender.Female));            
        }

        public void HairNext()
        {
            if (Character.HairToggle && (!Character.HeadAllElementsToggle || Character.Body[BodyItems.HeadAllElements].Position == 0))
            {
                ActivateItem(Character.ChangeNext(BodyItems.Hair));
            }
        }

        public void HairPrevios()
        {
            if (Character.HairToggle && !Character.HeadAllElementsToggle || Character.Body[BodyItems.HeadAllElements].Position == 0)
            {
                ActivateItem(Character.ChangePrevious(BodyItems.Hair));
            }
        }

        public void HairToggle(bool value)
        {
            Character.HairToggle = value;

            if (!Character.HeadAllElementsToggle)
            {
                ActivateItem(value ? Character.ChangeEnable(BodyItems.Hair) : Character.ChangeDisable(BodyItems.Hair));
            }
        }

        public void FacialHairNext()
        {
            if (Character.FacialHairToggle && !Character.HeadAllElementsToggle)
            {
                ActivateItem(Character.ChangeNext(BodyItems.FacialHair));
            }
        }

        public void FacialHairPrevios()
        {
            if (Character.FacialHairToggle && !Character.HeadAllElementsToggle)
            {
                ActivateItem(Character.ChangePrevious(BodyItems.FacialHair));
            }
        }

        public void FacialHairToggle(bool value)
        {
            Character.FacialHairToggle = value;
            ActivateItem(value ? Character.ChangeEnable(BodyItems.FacialHair) : Character.ChangeDisable(BodyItems.FacialHair));
        }

        public void TorsoNext()
        {
            ActivateItem(Character.ChangeNext(BodyItems.Torso));            
        }

        public void TorsoPrevios()
        {
            ActivateItem(Character.ChangePrevious(BodyItems.Torso));
        }

        public void Leg_LeftNext()
        {
            ActivateItem(Character.ChangeNext(BodyItems.Leg_Left));
        }

        public void Leg_LeftPrevios()
        {
            ActivateItem(Character.ChangePrevious(BodyItems.Leg_Left));
        }

        public void Leg_RightNext()
        {
            ActivateItem(Character.ChangeNext(BodyItems.Leg_Right));
        }

        public void Leg_RightPrevios()
        {
            ActivateItem(Character.ChangePrevious(BodyItems.Leg_Right));
        }

        public void ArmUpper_LeftNext()
        {
            ActivateItem(Character.ChangeNext(BodyItems.ArmUpper_Left));
        }

        public void ArmUpper_LeftPrevios()
        {
            ActivateItem(Character.ChangePrevious(BodyItems.ArmUpper_Left));
        }

        public void ArmUpper_RightNext()
        {
            ActivateItem(Character.ChangeNext(BodyItems.ArmUpper_Right));
        }

        public void ArmUpper_RightPrevios()
        {
            ActivateItem(Character.ChangePrevious(BodyItems.ArmUpper_Right));
        }

        public void ArmLower_LeftNext()
        {
            ActivateItem(Character.ChangeNext(BodyItems.ArmLower_Left));
        }

        public void ArmLower_LeftPrevios()
        {
            ActivateItem(Character.ChangePrevious(BodyItems.ArmLower_Left));
        }

        public void ArmLower_RightNext()
        {
            ActivateItem(Character.ChangeNext(BodyItems.ArmLower_Right));
        }

        public void ArmLower_RightPrevios()
        {
            ActivateItem(Character.ChangePrevious(BodyItems.ArmLower_Right));
        }

        public void Hand_LeftNext()
        {
            ActivateItem(Character.ChangeNext(BodyItems.Hand_Left));
        }

        public void Hand_LeftPrevios()
        {
            ActivateItem(Character.ChangePrevious(BodyItems.Hand_Left));
        }

        public void Hand_RightNext()
        {
            ActivateItem(Character.ChangeNext(BodyItems.Hand_Right));
        }

        public void Hand_RightPrevios()
        {
            ActivateItem(Character.ChangePrevious(BodyItems.Hand_Right));
        }

        public void HipsNext()
        {
            ActivateItem(Character.ChangeNext(BodyItems.Hips));
        }

        public void HipsPrevios()
        {
            ActivateItem(Character.ChangePrevious(BodyItems.Hips));
        }

        public void EyeBrowNext()
        {
            if (!Character.HeadAllElementsToggle)
            {
                ActivateItem(Character.ChangeNext(BodyItems.EyeBrow));
            }
        }

        public void EyeBrowPrevios()
        {
            if (!Character.HeadAllElementsToggle)
            {
                ActivateItem(Character.ChangePrevious(BodyItems.EyeBrow));
            }
        }

        public void EarNext()
        {

            if (Character.Race == Race.Elf 
                && (!Character.HeadAllElementsToggle 
                || Character.Body[BodyItems.HeadAllElements].Position == 0))
            {
                ActivateItem(Character.ChangeNext(BodyItems.Ear));
            }
        }

        public void EarPrevios()
        {
            if (Character.Race == Race.Elf 
                && (!Character.HeadAllElementsToggle 
                || Character.Body[BodyItems.HeadAllElements].Position == 0))
            {
                ActivateItem(Character.ChangePrevious(BodyItems.Ear));
            }
        }

        public void HeadNext()
        {
            if (Character.HeadAllElementsToggle)
            {
                ActivateItem(Character.ChangeNext(BodyItems.HeadNoElements));

                if (Character.Body.TryGetValue(BodyItems.HeadNoElements, out var _item)
                    && _item.Position == 0)
                {
                    ActivateItem(Character.ChangeEnable(BodyItems.Hair));

                    if (Character.Race == Race.Elf)
                    {
                        ActivateItem(Character.ChangeEnable(BodyItems.Ear));
                    }
                }
                else
                {
                    ActivateItem(Character.ChangeDisable(BodyItems.Hair));

                    if (Character.Race == Race.Elf)
                    {
                        ActivateItem(Character.ChangeDisable(BodyItems.Ear));
                    }
                }
            }
            else
            {
                ActivateItem(Character.ChangeNext(BodyItems.HeadAllElements));
            }
        }

        public void HeadPrevios()
        {
            if (Character.HeadAllElementsToggle)
            {
                ActivateItem(Character.ChangePrevious(BodyItems.HeadNoElements));


                if (Character.Body.TryGetValue(BodyItems.HeadNoElements, out var _item) 
                    && _item.Position == 0)
                {
                    ActivateItem(Character.ChangeEnable(BodyItems.Hair));

                    if (Character.Race == Race.Elf)
                    {
                        ActivateItem(Character.ChangeEnable(BodyItems.Ear));
                    }
                }
                else
                {
                    ActivateItem(Character.ChangeDisable(BodyItems.Hair));

                    if (Character.Race == Race.Elf)
                    {
                        ActivateItem(Character.ChangeDisable(BodyItems.Ear));
                    }
                }
            }
            else
            {
                ActivateItem(Character.ChangePrevious(BodyItems.HeadAllElements));
            }
        }

        public void HeadToggle(bool value)
        {
            if (!Character.HeadCov1Toggle || !Character.HeadCov2Toggle || !Character.HeadCov3Toggle)
            {
                Character.HeadAllElementsToggle = value;
                ActivateItem(value ? Character.ChangeDisable(BodyItems.HeadAllElements) : Character.ChangeDisable(BodyItems.HeadNoElements));
                ActivateItem(value ? Character.ChangeEnable(BodyItems.HeadNoElements) : Character.ChangeEnable(BodyItems.HeadAllElements));
         
                if (Character.Body[BodyItems.HeadAllElements].Position == 0)
                {
                    ActivateItem(Character.ChangeEnable(BodyItems.Hair));
                    ActivateItem(Character.ChangeEnable(BodyItems.Ear));
                }
                else
                {
                    ActivateItem(value ? Character.ChangeDisable(BodyItems.Hair) : Character.ChangeEnable(BodyItems.Hair));
                    ActivateItem(value ? Character.ChangeDisable(BodyItems.Ear) : Character.ChangeEnable(BodyItems.Ear));
                }

                ActivateItem(value ? Character.ChangeDisable(BodyItems.EyeBrow) : Character.ChangeEnable(BodyItems.EyeBrow));
                ActivateItem(value ? Character.ChangeDisable(BodyItems.FacialHair) : Character.ChangeEnable(BodyItems.FacialHair));
            }
        }

        public void HeadCov1Next()
        {
            if (Character.HeadCov1Toggle)
            {
                ActivateItem(Character.ChangeNext(BodyItems.HeadCoverings_No_FacialHair));
            }
        }

        public void HeadCov1Previos()
        {
            if (Character.HeadCov1Toggle)
            {
                ActivateItem(Character.ChangePrevious(BodyItems.HeadCoverings_No_FacialHair));
            }
        }

        public void HeadCov1Toggle(bool value)
        {
            if (!Character.HeadAllElementsToggle)
            {
                Character.HeadCov1Toggle = value;
                ActivateItem(value ? Character.ChangeEnable(BodyItems.HeadCoverings_No_FacialHair) : Character.ChangeDisable(BodyItems.HeadCoverings_No_FacialHair));
            }
        }

        public void HeadCov2Next()
        {

            if (Character.HeadCov2Toggle)
            {
                ActivateItem(Character.ChangeNext(BodyItems.HeadCoverings_Base_Hair));
            }
        }

        public void HeadCov2Previos()
        {
            if (Character.HeadCov2Toggle)
            {
                ActivateItem(Character.ChangePrevious(BodyItems.HeadCoverings_Base_Hair));
            }
        }

        public void HeadCov2Toggle(bool value)
        {
            if (!Character.HeadAllElementsToggle)
            {
                Character.HeadCov2Toggle = value;
                ActivateItem(value ? Character.ChangeEnable(BodyItems.HeadCoverings_Base_Hair) : Character.ChangeDisable(BodyItems.HeadCoverings_Base_Hair));
            }
        }

        public void HeadCov3Next()
        {
            if (Character.HeadCov3Toggle)
            {
                ActivateItem(Character.ChangeNext(BodyItems.HeadCoverings_No_Hair));
            }
        }

        public void HeadCov3Previos()
        {
            if (Character.HeadCov3Toggle)
            {
                ActivateItem(Character.ChangePrevious(BodyItems.HeadCoverings_No_Hair));
            }
        }

        public void HeadCov3Toggle(bool value)
        {
            if (!Character.HeadAllElementsToggle)
            {
                Character.HeadCov3Toggle = value;
                ActivateItem(value ? Character.ChangeEnable(BodyItems.HeadCoverings_No_Hair) : Character.ChangeDisable(BodyItems.HeadCoverings_No_Hair));
            }
        }

        public void ChestNext()
        {
            if (Character.ChestToggle)
            {
                //ActivateItem(Character.ChangeNext(BodyItems.Chest_Attachment));
            }
        }

        public void ChestPrevios()
        {
            if (Character.ChestToggle)
            {
                //ActivateItem(Character.ChangePrevious(BodyItems.Chest_Attachment));
            }
        }

        public void ChestToggle(bool value)
        {
            Character.ChestToggle = value;
            //ActivateItem(value ? Character.ChangeEnable(BodyItems.Chest_Attachment) : Character.ChangeDisable(BodyItems.Chest_Attachment));
        }

        public void SholderRNext()
        {
            if (Character.SholderRToggle)
            {
                ActivateItem(Character.ChangeNext(BodyItems.Shoulder_Attachment_Right));
            }
        }

        public void SholderRPrevios()
        {
            if (Character.SholderRToggle)
            {
                ActivateItem(Character.ChangePrevious(BodyItems.Shoulder_Attachment_Right));
            }
        }

        public void SholderRToggle(bool value)
        {
            Character.SholderRToggle = value;
            ActivateItem(value ? Character.ChangeEnable(BodyItems.Shoulder_Attachment_Right) : Character.ChangeDisable(BodyItems.Shoulder_Attachment_Right));
        }

        public void SholderLNext()
        {
            if (Character.SholderLToggle)
            {
                ActivateItem(Character.ChangeNext(BodyItems.Shoulder_Attachment_Left));
            }
        }

        public void SholderLPrevios()
        {
            if (Character.SholderLToggle)
            {
                ActivateItem(Character.ChangePrevious(BodyItems.Shoulder_Attachment_Left));
            }
        }

        public void SholderLToggle(bool value)
        {
            Character.SholderLToggle = value;
            ActivateItem(value ? Character.ChangeEnable(BodyItems.Shoulder_Attachment_Left) : Character.ChangeDisable(BodyItems.Shoulder_Attachment_Left));
        }

        public void ElbowRNext()
        {
            if (Character.ElbowRToggle)
            {
                ActivateItem(Character.ChangeNext(BodyItems.Elbow_Attachment_Right));
            }
        }

        public void ElbowRPrevios()
        {
            if (Character.ElbowRToggle)
            {
                ActivateItem(Character.ChangePrevious(BodyItems.Elbow_Attachment_Right));
            }
        }

        public void ElbowRToggle(bool value)
        {
            Character.ElbowRToggle = value;
            ActivateItem(value ? Character.ChangeEnable(BodyItems.Elbow_Attachment_Right) : Character.ChangeDisable(BodyItems.Elbow_Attachment_Right));
        }

        public void ElbowLNext()
        {
            if (Character.ElbowLToggle)
            {
                ActivateItem(Character.ChangeNext(BodyItems.Elbow_Attachment_Left));
            }
        }

        public void ElbowLPrevios()
        {
            if (Character.ElbowLToggle)
            {
                ActivateItem(Character.ChangePrevious(BodyItems.Elbow_Attachment_Left));
            }
        }

        public void ElbowLToggle(bool value)
        {
            Character.ElbowLToggle = value;
            ActivateItem(value ? Character.ChangeEnable(BodyItems.Elbow_Attachment_Left) : Character.ChangeDisable(BodyItems.Elbow_Attachment_Left));
        }

        public void KneeRNext()
        {
            if (Character.KneeRToggle)
            {
                ActivateItem(Character.ChangeNext(BodyItems.Knee_Attachement_Right));
            }
        }

        public void KneeRPrevios()
        {
            if (Character.KneeRToggle)
            {
                ActivateItem(Character.ChangePrevious(BodyItems.Knee_Attachement_Right));
            }
        }

        public void KneeRToggle(bool value)
        {
            Character.KneeRToggle = value;
            ActivateItem(value ? Character.ChangeEnable(BodyItems.Knee_Attachement_Right) : Character.ChangeDisable(BodyItems.Knee_Attachement_Right));
        }

        public void KneeLNext()
        {
            if (Character.KneeLToggle)
            {
                ActivateItem(Character.ChangeNext(BodyItems.Knee_Attachement_Left));
            }
        }

        public void KneeLPrevios()
        {
            if (Character.KneeLToggle)
            {
                ActivateItem(Character.ChangePrevious(BodyItems.Knee_Attachement_Left));
            }
        }

        public void KneeLToggle(bool value)
        {
            Character.KneeLToggle = value;
            ActivateItem(value ? Character.ChangeEnable(BodyItems.Knee_Attachement_Left) : Character.ChangeDisable(BodyItems.Knee_Attachement_Left));
        }

        public void HipsANext()
        {
            if (Character.HipsAToggle)
            {
                ActivateItem(Character.ChangeNext(BodyItems.Hips_Attachment));
            }
        }

        public void HipsAPrevios()
        {
            if (Character.HipsAToggle)
            {
                ActivateItem(Character.ChangePrevious(BodyItems.Hips_Attachment));
            }
        }

        public void HipsAToggle(bool value)
        {
            Character.HipsAToggle = value;
            ActivateItem(value ? Character.ChangeEnable(BodyItems.Hips_Attachment) : Character.ChangeDisable(BodyItems.Hips_Attachment));
        }

        public void Previos()
        {
            if (PrevCharacters.Count > 0)
            {
                ActivateItem(Character.ClearAllCharacter());

                NextCharacters.Push(Character);

                Character = PrevCharacters.Pop();

                ActivateItem(Character.ReadAllCharacter());
            }
        }

        public void Next()
        {
            if (NextCharacters.Count > 0)
            {
                ActivateItem(Character.ClearAllCharacter());

                PrevCharacters.Push(Character);

                Character = NextCharacters.Pop();

                ActivateItem(Character.ReadAllCharacter());
            }
        }



        // randomize character creating button
        void OnGUI()
        {
            /*
            if (GUI.Button(new Rect(10, 10, 150, 50), "Randomize Character"))
            {
                // call randomization method
                Randomize();
            }
            */

            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.fontStyle = FontStyle.Bold;
            style.fontSize = 24;
            GUI.Label(new Rect(10, 10, 150, 50), "Hold Right Mouse Button Down\nor use W A S D To Rotate.", style);
        }

        private void Start()
        {
            // rebuild all lists
            BuildLists();

            // disable any enabled objects before clear
            if (enabledObjects.Count != 0)
            {
                foreach (GameObject g in enabledObjects)
                {
                    g.SetActive(false);
                }
            }

            // clear enabled objects list
            enabledObjects.Clear();

            // set default male character
            //ActivateItem(Character.ClearAllCharacter());
            Character = new Character(male, female, allGender);
            ActivateItem(Character.ReadAllCharacter());
                        
            // setting up the camera position, rotation, and reference for use
            Transform cam = Camera.main.transform;
            if (cam)
            {
                cam.position = transform.position + new Vector3(0, 0.3f, 2);
                cam.rotation = Quaternion.Euler(0, -180, 0);
                camHolder = new GameObject().transform;
                camHolder.position = transform.position + new Vector3(0, 1, 0);
                cam.LookAt(camHolder);
                cam.SetParent(camHolder);
            }

            // if repeat on play is checked in the inspector, repeat the randomize method based on the shuffle speed, also defined in the inspector
            if (repeatOnPlay)
                InvokeRepeating("Randomize", shuffleSpeed, shuffleSpeed);
        }

        private void Update()
        {
            if (camHolder)
            {
                if (Input.GetKey(KeyCode.Mouse1))
                {
                    x += 1 * Input.GetAxis("Mouse X");
                    y -= 1 * Input.GetAxis("Mouse Y");
                    UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                    UnityEngine.Cursor.visible = false;
                }
                else
                {
                    x -= 1 * Input.GetAxis("Horizontal");
                    y -= 1 * Input.GetAxis("Vertical");
                    UnityEngine.Cursor.lockState = CursorLockMode.None;
                    UnityEngine.Cursor.visible = true;
                }
            }
        }

        void LateUpdate()
        {
            // method for handling the camera rotation around the character
            if (camHolder)
            {
                y = Mathf.Clamp(y, -45, 15);
                camHolder.eulerAngles = new Vector3(y, x, 0.0f);
            }
        }

        // character randomization method
        void Randomize()
        {
            if (Randomizer)
            {
                RandomCharacter();
            }
        }


        public void RandomCharacter()
        {
            PrevCharacters.Push(Character);
            // disable any enabled objects before clear
            ActivateItem(Character.ClearAllCharacter());

            Character = new Character(male, female, allGender);
            // initialize settings
            ActivateItem(Character.ChangeGender(Gender.Male));
            ActivateItem(Character.ChangeRace(Race.Human));
            //ElfToggle(false);
            SkinColor skinColor = SkinColor.White;
            Elements elements = Elements.Yes;
            BodyItems headCovering = BodyItems.HeadCoverings_Base_Hair;
            FacialHair facialHair = FacialHair.Yes;


            // roll for gender
            if (!GetPercent(50))
                ActivateItem(Character.ChangeGender(Gender.Female));

            // roll for human (70% chance, 30% chance for elf)
            if (!GetPercent(70))
            {
                ElfToggle(true);
                ActivateItem(Character.ChangeRace(Race.Elf));
            }
            else
            {
                ElfToggle(false);
            }

            // roll for facial elements (beard, eyebrows)
            if (!GetPercent(50))
                elements = Elements.No;

            // select head covering 33% chance for each
            int headCoveringRoll = Random.Range(0, 100);
            // HeadCoverings_Base_Hair
            if (headCoveringRoll <= 33)
                headCovering = BodyItems.HeadCoverings_Base_Hair;
            // HeadCoverings_No_FacialHair
            if (headCoveringRoll > 33 && headCoveringRoll < 66)
                headCovering = BodyItems.HeadCoverings_No_FacialHair;
            // HeadCoverings_No_Hair
            if (headCoveringRoll >= 66)
                headCovering = BodyItems.HeadCoverings_No_Hair;

            // select skin color if human, otherwise set skin color to elf
            switch (Character.Race)
            {
                case Race.Human:
                    // select human skin 33% chance for each
                    int colorRoll = Random.Range(0, 100);
                    // select white skin
                    if (colorRoll <= 33)
                        skinColor = SkinColor.White;
                    // select brown skin
                    if (colorRoll > 33 && colorRoll < 66)
                        skinColor = SkinColor.Brown;
                    // select black skin
                    if (colorRoll >= 66)
                        skinColor = SkinColor.Black;
                    break;
                case Race.Elf:
                    // select elf skin
                    skinColor = SkinColor.Elf;
                    break;
            }

            //roll for gender
            switch (Character.Gander)
            {
                case Gender.Male:
                    // roll for facial hair if male
                    if (!GetPercent(50))
                        facialHair = FacialHair.No;

                    // initialize randomization
                    RandomizeByVariable(male, Character.Gander, elements, Character.Race, facialHair, skinColor, headCovering);
                    break;

                case Gender.Female:

                    // no facial hair if female
                    facialHair = FacialHair.No;

                    // initialize randomization
                    RandomizeByVariable(female, Character.Gander, elements, Character.Race, facialHair, skinColor, headCovering);
                    break;
            }

        }

        // randomization method based on previously selected variables
        void RandomizeByVariable(CharacterObjectGroups cog, Gender gender, Elements elements, Race race, FacialHair facialHair, SkinColor skinColor, BodyItems headCovering)
        {
            Character.HairToggle = false;
            Character.FacialHairToggle = false;
            
            // if facial elements are enabled
            switch (elements)
            {
                case Elements.Yes:
                    //select head with all elements
                    if (Character.GenderDict[Character.Gander][BodyItems.HeadAllElements].Count != 0)
                    {
                        ActivateItem(Character.ChangeForIndex(BodyItems.HeadAllElements, Random.Range(0, Character.GenderDict[Character.Gander][BodyItems.HeadAllElements].Count)));

                    }

                    //select eyebrows
                    if (Character.GenderDict[Character.Gander][BodyItems.EyeBrow].Count != 0)
                        ActivateItem(Character.ChangeForIndex(BodyItems.EyeBrow, Random.Range(0, Character.GenderDict[Character.Gander][BodyItems.EyeBrow].Count)));

                    //select facial hair (conditional)
                    if (Character.GenderDict[Character.Gander][BodyItems.FacialHair].Count != 0 && facialHair == FacialHair.Yes && gender == Gender.Male && headCovering != BodyItems.HeadCoverings_No_FacialHair)
                    {
                        ActivateItem(Character.ChangeForIndex(BodyItems.FacialHair, Random.Range(0, Character.GenderDict[Character.Gander][BodyItems.FacialHair].Count)));
                        Character.FacialHairToggle = true;
                    }
                        

                    // select hair attachment
                    switch (headCovering)
                    {
                        case BodyItems.HeadCoverings_Base_Hair:
                            // set hair attachment to index 1
                            if (Character.NoGenderDict[BodyItems.Hair].Count != 0)
                                ActivateItem(Character.ChangeForIndex(BodyItems.Hair, Random.Range(0, Character.NoGenderDict[BodyItems.Hair].Count)));
                            if (Character.NoGenderDict[BodyItems.HeadCoverings_Base_Hair].Count != 0)
                                ActivateItem(Character.ChangeForIndex(BodyItems.HeadCoverings_Base_Hair, Random.Range(0, Character.NoGenderDict[BodyItems.HeadCoverings_Base_Hair].Count)));
                            Character.HairToggle = true;
                            break;
                        case BodyItems.HeadCoverings_No_FacialHair:
                            // no facial hair attachment
                            if (Character.NoGenderDict[BodyItems.Hair].Count != 0)
                                ActivateItem(Character.ChangeForIndex(BodyItems.Hair, Random.Range(0, Character.NoGenderDict[BodyItems.Hair].Count)));
                            if (Character.NoGenderDict[BodyItems.HeadCoverings_No_FacialHair].Count != 0)
                                ActivateItem(Character.ChangeForIndex(BodyItems.HeadCoverings_No_FacialHair, Random.Range(0, Character.NoGenderDict[BodyItems.HeadCoverings_No_FacialHair].Count)));
                            Character.HairToggle = true;
                            break;
                        case BodyItems.HeadCoverings_No_Hair:
                            // select hair attachment
                            if (Character.NoGenderDict[BodyItems.HeadCoverings_No_Hair].Count != 0)
                                ActivateItem(Character.ChangeForIndex(BodyItems.HeadCoverings_No_Hair, Random.Range(0, Character.NoGenderDict[BodyItems.HeadCoverings_No_Hair].Count)));
                            // if not human
                            if (race != Race.Human)
                            {
                                // select elf ear attachment
                                if (Character.NoGenderDict[BodyItems.Ear].Count != 0)
                                    ActivateItem(Character.ChangeForIndex(BodyItems.Ear, Random.Range(0, Character.NoGenderDict[BodyItems.Ear].Count)));
                            }
                            break;
                    }
                    break;

                case Elements.No:
                    //select head with no elements
                    if (Character.GenderDict[Character.Gander][BodyItems.HeadNoElements].Count != 0)
                        ActivateItem(Character.ChangeForIndex(BodyItems.HeadNoElements, Random.Range(0, Character.GenderDict[Character.Gander][BodyItems.HeadNoElements].Count)));
                    break;
            }

            // select torso starting at index 1
            if (Character.GenderDict[Character.Gander][BodyItems.Torso].Count != 0)
                ActivateItem(Character.ChangeForIndex(BodyItems.Torso, Random.Range(1, Character.GenderDict[Character.Gander][BodyItems.Torso].Count)));

            // determine chance for upper arms to be different and activate
            if (Character.GenderDict[Character.Gander][BodyItems.ArmUpper_Right].Count != 0)
                RandomizeLeftRight(BodyItems.ArmUpper_Right, BodyItems.ArmUpper_Left, 15);

            // determine chance for lower arms to be different and activate
            if (Character.GenderDict[Character.Gander][BodyItems.ArmLower_Right].Count != 0)
                RandomizeLeftRight(BodyItems.ArmLower_Right, BodyItems.ArmLower_Left, 15);

            // determine chance for hands to be different and activate
            if (Character.GenderDict[Character.Gander][BodyItems.Hand_Right].Count != 0)
                RandomizeLeftRight(BodyItems.Hand_Right, BodyItems.Hand_Left, 15);

            // select hips starting at index 1
            if (Character.GenderDict[Character.Gander][BodyItems.Hips].Count != 0)
                ActivateItem(Character.ChangeForIndex(BodyItems.Hips, Random.Range(1, Character.GenderDict[Character.Gander][BodyItems.Hips].Count)));

            // determine chance for legs to be different and activate
            if (Character.GenderDict[Character.Gander][BodyItems.Leg_Right].Count != 0)
                RandomizeLeftRight(BodyItems.Leg_Right, BodyItems.Leg_Left, 15);

            // select chest attachment
            if (Character.NoGenderDict[BodyItems.Chest_Attachment].Count != 0)
                ActivateItem(Character.ChangeForIndex(BodyItems.Chest_Attachment, Random.Range(0, Character.NoGenderDict[BodyItems.Chest_Attachment].Count)));

            // select back attachment
            if (Character.NoGenderDict[BodyItems.Back_Attachment].Count != 0)
                ActivateItem(Character.ChangeForIndex(BodyItems.Back_Attachment, Random.Range(0, Character.NoGenderDict[BodyItems.Back_Attachment].Count)));

            // determine chance for shoulder attachments to be different and activate
            if (Character.NoGenderDict[BodyItems.Shoulder_Attachment_Right].Count != 0)
                RandomizeLeftRight(BodyItems.Shoulder_Attachment_Right, BodyItems.Shoulder_Attachment_Left, 10);

            // determine chance for elbow attachments to be different and activate
            if (Character.NoGenderDict[BodyItems.Elbow_Attachment_Right].Count != 0)
                RandomizeLeftRight(BodyItems.Elbow_Attachment_Right, BodyItems.Elbow_Attachment_Left, 10);

            // select hip attachment
            if (Character.NoGenderDict[BodyItems.Hips_Attachment].Count != 0)
                ActivateItem(Character.ChangeForIndex(BodyItems.Hips_Attachment, Random.Range(0, Character.NoGenderDict[BodyItems.Hips_Attachment].Count)));

            // determine chance for knee attachments to be different and activate
            if (Character.NoGenderDict[BodyItems.Knee_Attachement_Right].Count != 0)
                RandomizeLeftRight(BodyItems.Knee_Attachement_Right, BodyItems.Knee_Attachement_Left, 10);

            // start randomization of the random characters colors
            RandomizeColors(skinColor);
        }

        // handle randomization of the random characters colors
        void RandomizeColors(SkinColor skinColor)
        {
            // set skin and hair colors based on skin color roll
            switch (skinColor)
            {
                case SkinColor.White:
                    // randomize and set white skin, hair, stubble, and scar color
                    RandomizeAndSetHairSkinColors("White", whiteSkin, whiteHair, whiteStubble, whiteScar);
                    break;

                case SkinColor.Brown:
                    // randomize and set brown skin, hair, stubble, and scar color
                    RandomizeAndSetHairSkinColors("Brown", brownSkin, brownHair, brownStubble, brownScar);
                    break;

                case SkinColor.Black:
                    // randomize and black elf skin, hair, stubble, and scar color
                    RandomizeAndSetHairSkinColors("Black", blackSkin, blackHair, blackStubble, blackScar);
                    break;

                case SkinColor.Elf:
                    // randomize and set elf skin, hair, stubble, and scar color
                    RandomizeAndSetHairSkinColors("Elf", elfSkin, elfHair, elfStubble, elfScar);
                    break;
            }

            // randomize and set primary color
            if (primary.Length != 0)
                mat.SetColor("_Color_Primary", primary[Random.Range(0, primary.Length)]);
            else
                Debug.Log("No Primary Colors Specified In The Inspector");

            // randomize and set secondary color
            if (secondary.Length != 0)
                mat.SetColor("_Color_Secondary", secondary[Random.Range(0, secondary.Length)]);
            else
                Debug.Log("No Secondary Colors Specified In The Inspector");

            // randomize and set primary metal color
            if (metalPrimary.Length != 0)
                mat.SetColor("_Color_Metal_Primary", metalPrimary[Random.Range(0, metalPrimary.Length)]);
            else
                Debug.Log("No Primary Metal Colors Specified In The Inspector");

            // randomize and set secondary metal color
            if (metalSecondary.Length != 0)
                mat.SetColor("_Color_Metal_Secondary", metalSecondary[Random.Range(0, metalSecondary.Length)]);
            else
                Debug.Log("No Secondary Metal Colors Specified In The Inspector");

            // randomize and set primary leather color
            if (leatherPrimary.Length != 0)
                mat.SetColor("_Color_Leather_Primary", leatherPrimary[Random.Range(0, leatherPrimary.Length)]);
            else
                Debug.Log("No Primary Leather Colors Specified In The Inspector");

            // randomize and set secondary leather color
            if (leatherSecondary.Length != 0)
                mat.SetColor("_Color_Leather_Secondary", leatherSecondary[Random.Range(0, leatherSecondary.Length)]);
            else
                Debug.Log("No Secondary Leather Colors Specified In The Inspector");

            // randomize and set body art color
            if (bodyArt.Length != 0)
                mat.SetColor("_Color_BodyArt", bodyArt[Random.Range(0, bodyArt.Length)]);
            else
                Debug.Log("No Body Art Colors Specified In The Inspector");

            // randomize and set body art amount
            mat.SetFloat("_BodyArt_Amount", Random.Range(0.0f, 1.0f));
        }

        void RandomizeAndSetHairSkinColors(string info, Color[] skin, Color[] hair, Color stubble, Color scar)
        {
            // randomize and set elf skin color
            if (skin.Length != 0)
            {
                mat.SetColor("_Color_Skin", skin[Random.Range(0, skin.Length)]);
            }
            else
            {
                Debug.Log("No " + info + " Skin Colors Specified In The Inspector");
            }

            // randomize and set elf hair color
            if (hair.Length != 0)
            {
                mat.SetColor("_Color_Hair", hair[Random.Range(0, hair.Length)]);
            }
            else
            {
                Debug.Log("No " + info + " Hair Colors Specified In The Inspector");
            }

            // set stubble color
            mat.SetColor("_Color_Stubble", stubble);

            // set scar color
            mat.SetColor("_Color_Scar", scar);
        }

        // method for handling the chance of left/right items to be differnt (such as shoulders, hands, legs, arms)
        void RandomizeLeftRight(BodyItems objectRight, BodyItems objectLeft, int rndPercent)
        {
            // rndPercent = chance for left item to be different

            // stored right index
            
            int index = Random.Range(0, Character.GenderHash.Contains(objectRight) 
                        ? Character.GenderDict[Character.Gander][objectRight].Count
                        : Character.NoGenderDict[objectRight].Count);

            // enable item from list using index
            ActivateItem(Character.ChangeForIndex(objectRight, index));

            // roll for left item mismatch, if true randomize index based on left item list
            if (GetPercent(rndPercent))
                index = Random.Range(0, Character.GenderHash.Contains(objectLeft)
                                ? Character.GenderDict[Character.Gander][objectLeft].Count
                                : Character.NoGenderDict[objectLeft].Count);

            // enable left item from list using index
            ActivateItem(Character.ChangeForIndex(objectLeft, index));
        }

        // enable game object and add it to the enabled objects list
        void ActivateItem(GameObject firstObj)
        {
            // enable item
            firstObj.SetActive(true);

            // add item to the enabled items list
            enabledObjects.Add(firstObj);
        }

        void ActivateItem(List<Tuple<ActForItem, List<Item>>> _objList)
        {
            foreach(var _act in _objList)
            {
                if(_act.Item1 == ActForItem.Add)
                {
                    foreach(Item _item in _act.Item2)
                    {
                        // enable item
                        if (_item.Object != null)
                        {
                            while (_item.Object.activeSelf == !_item.Visible)
                            {
                                _item.Object.SetActive(_item.Visible);
                            }
                        }
                    }
                }
                if (_act.Item1 == ActForItem.Remove)
                {
                    foreach(Item _item in _act.Item2)
                    {
                        // disable item
                        if (_item.Object != null)
                        {
                            while (_item.Object.activeSelf)
                            {
                                _item.Object.SetActive(false);
                            }
                        }
                    }
                }
            }
        }

        Color ConvertColor(int r, int g, int b)
        {
            return new Color(r / 255.0f, g / 255.0f, b / 255.0f, 1);
        }

        // method for rolling percentages (returns true/false)
        bool GetPercent(int pct)
        {
            bool p = false;
            int roll = Random.Range(0, 100);
            if (roll <= pct)
            {
                p = true;
            }
            return p;
        }

        // build all item lists for use in randomization
        private void BuildLists()
        {
            //build out male lists
            BuildList(male.headAllElements, "Male_Head_All_Elements");
            BuildList(male.headNoElements, "Male_Head_No_Elements");
            BuildList(male.eyebrow, "Male_01_Eyebrows");
            BuildList(male.facialHair, "Male_02_FacialHair");
            BuildList(male.torso, "Male_03_Torso");
            BuildList(male.arm_Upper_Right, "Male_04_Arm_Upper_Right");
            BuildList(male.arm_Upper_Left, "Male_05_Arm_Upper_Left");
            BuildList(male.arm_Lower_Right, "Male_06_Arm_Lower_Right");
            BuildList(male.arm_Lower_Left, "Male_07_Arm_Lower_Left");
            BuildList(male.hand_Right, "Male_08_Hand_Right");
            BuildList(male.hand_Left, "Male_09_Hand_Left");
            BuildList(male.hips, "Male_10_Hips");
            BuildList(male.leg_Right, "Male_11_Leg_Right");
            BuildList(male.leg_Left, "Male_12_Leg_Left");

            //build out female lists
            BuildList(female.headAllElements, "Female_Head_All_Elements");
            BuildList(female.headNoElements, "Female_Head_No_Elements");
            BuildList(female.eyebrow, "Female_01_Eyebrows");
            BuildList(female.facialHair, "Female_02_FacialHair");
            BuildList(female.torso, "Female_03_Torso");
            BuildList(female.arm_Upper_Right, "Female_04_Arm_Upper_Right");
            BuildList(female.arm_Upper_Left, "Female_05_Arm_Upper_Left");
            BuildList(female.arm_Lower_Right, "Female_06_Arm_Lower_Right");
            BuildList(female.arm_Lower_Left, "Female_07_Arm_Lower_Left");
            BuildList(female.hand_Right, "Female_08_Hand_Right");
            BuildList(female.hand_Left, "Female_09_Hand_Left");
            BuildList(female.hips, "Female_10_Hips");
            BuildList(female.leg_Right, "Female_11_Leg_Right");
            BuildList(female.leg_Left, "Female_12_Leg_Left");

            // build out all gender lists
            BuildList(allGender.all_Hair, "All_01_Hair");
            BuildList(allGender.all_Head_Attachment, "All_02_Head_Attachment");
            BuildList(allGender.headCoverings_Base_Hair, "HeadCoverings_Base_Hair");
            BuildList(allGender.headCoverings_No_FacialHair, "HeadCoverings_No_FacialHair");
            BuildList(allGender.headCoverings_No_Hair, "HeadCoverings_No_Hair");
            BuildList(allGender.chest_Attachment, "All_03_Chest_Attachment");
            BuildList(allGender.back_Attachment, "All_04_Back_Attachment");
            BuildList(allGender.shoulder_Attachment_Right, "All_05_Shoulder_Attachment_Right");
            BuildList(allGender.shoulder_Attachment_Left, "All_06_Shoulder_Attachment_Left");
            BuildList(allGender.elbow_Attachment_Right, "All_07_Elbow_Attachment_Right");
            BuildList(allGender.elbow_Attachment_Left, "All_08_Elbow_Attachment_Left");
            BuildList(allGender.hips_Attachment, "All_09_Hips_Attachment");
            BuildList(allGender.knee_Attachement_Right, "All_10_Knee_Attachement_Right");
            BuildList(allGender.knee_Attachement_Left, "All_11_Knee_Attachement_Left");
            BuildList(allGender.elf_Ear, "Elf_Ear");
        }

        // called from the BuildLists method
        void BuildList(List<GameObject> targetList, string characterPart)
        {
            Transform[] rootTransform = gameObject.GetComponentsInChildren<Transform>();

            // declare target root transform
            Transform targetRoot = null;

            // find character parts parent object in the scene
            foreach (Transform t in rootTransform)
            {
                if (t.gameObject.name == characterPart)
                {
                    targetRoot = t;
                    break;
                }
            }

            // clears targeted list of all objects
            targetList.Clear();

            // cycle through all child objects of the parent object
            for (int i = 0; i < targetRoot.childCount; i++)
            {
                // get child gameobject index i
                GameObject go = targetRoot.GetChild(i).gameObject;

                // disable child object
                go.SetActive(false);

                // add object to the targeted object list
                targetList.Add(go);

                // collect the material for the random character, only if null in the inspector;
                if (!mat)
                {
                    if (go.GetComponent<SkinnedMeshRenderer>())
                        mat = go.GetComponent<SkinnedMeshRenderer>().material;
                }
            }
        }
    }

    // classe for keeping the lists organized, allows for simple switching from male/female objects
    [System.Serializable]
    public class CharacterObjectGroups
    {
        public List<GameObject> headAllElements;
        public List<GameObject> headNoElements;
        public List<GameObject> eyebrow;
        public List<GameObject> facialHair;
        public List<GameObject> torso;
        public List<GameObject> arm_Upper_Right;
        public List<GameObject> arm_Upper_Left;
        public List<GameObject> arm_Lower_Right;
        public List<GameObject> arm_Lower_Left;
        public List<GameObject> hand_Right;
        public List<GameObject> hand_Left;
        public List<GameObject> hips;
        public List<GameObject> leg_Right;
        public List<GameObject> leg_Left;
    }

    // classe for keeping the lists organized, allows for organization of the all gender items
    [System.Serializable]
    public class CharacterObjectListsAllGender
    {
        public List<GameObject> headCoverings_Base_Hair;
        public List<GameObject> headCoverings_No_FacialHair;
        public List<GameObject> headCoverings_No_Hair;
        public List<GameObject> all_Hair;
        public List<GameObject> all_Head_Attachment;
        public List<GameObject> chest_Attachment;
        public List<GameObject> back_Attachment;
        public List<GameObject> shoulder_Attachment_Right;
        public List<GameObject> shoulder_Attachment_Left;
        public List<GameObject> elbow_Attachment_Right;
        public List<GameObject> elbow_Attachment_Left;
        public List<GameObject> hips_Attachment;
        public List<GameObject> knee_Attachement_Right;
        public List<GameObject> knee_Attachement_Left;
        public List<GameObject> all_12_Extra;
        public List<GameObject> elf_Ear;
    }
}