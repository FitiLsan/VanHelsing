using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


namespace BeastHunter
{
    [Serializable]
    public enum Gender { Male, Female }
    [Serializable]
    public enum Race { Human, Elf }
    public enum SkinColor { White, Brown, Black, Elf }
    public enum ElementSkinColor { Skin, Hair, Stubble, Scar }
    public enum ElementsColor { Gear, Metal, Leather, BodyArtColors }
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

    public class CharacterPresentationModel
    {
        #region Fields

        public Race Race = Race.Human;
        public Gender Gander = Gender.Male;
        public SkinColor SkinColor = SkinColor.White;
        public Dictionary<BodyItems, ItemBody> Body = new Dictionary<BodyItems, ItemBody>();
        public static Dictionary<Gender, Dictionary<BodyItems, List<GameObject>>> GenderDict = new Dictionary<Gender, Dictionary<BodyItems, List<GameObject>>>();
        public static Dictionary<BodyItems, List<GameObject>> NoGenderDict = new Dictionary<BodyItems, List<GameObject>>();
        public static Dictionary<SkinColor, Dictionary<ElementSkinColor, List<Color>>> SkinColorDict = new Dictionary<SkinColor, Dictionary<ElementSkinColor, List<Color>>>();
        public static Dictionary<SkinColor, Dictionary<ElementsColor, List<Color>>> ElementsColorDict = new Dictionary<SkinColor, Dictionary<ElementsColor, List<Color>>>();

        public ColorationSkin Color;

        public Material Material;

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


        #endregion


        #region Metods

        public CharacterPresentationModel(ColorationSkin _color, Material _material)
        {
            Color = _color;
            Material = _material;

            LoadColor();

            GenderHash = new HashSet<BodyItems>
            {
                BodyItems.HeadAllElements, BodyItems.HeadNoElements, BodyItems.EyeBrow, BodyItems.FacialHair, BodyItems.Torso,
                BodyItems.ArmUpper_Right, BodyItems.ArmUpper_Left, BodyItems.ArmLower_Right, BodyItems.ArmLower_Left,
                BodyItems.Hand_Right, BodyItems.Hand_Left, BodyItems.Hips, BodyItems.Leg_Right, BodyItems.Leg_Left
            };

            Body.Add(BodyItems.HeadAllElements, new ItemBody(GenderDict[Gander][BodyItems.HeadAllElements].First(), 0, true));
            Body.Add(BodyItems.HeadNoElements, new ItemBody(GenderDict[Gander][BodyItems.HeadNoElements].First(), 0, false));
            Body.Add(BodyItems.FacialHair, new ItemBody(GenderDict[Gander][BodyItems.FacialHair].First(), 0, true));
            Body.Add(BodyItems.Hair, new ItemBody(NoGenderDict[BodyItems.Hair].First(), 0, true));
            Body.Add(BodyItems.Ear, new ItemBody(NoGenderDict[BodyItems.Ear].First(), 0, false));
            Body.Add(BodyItems.EyeBrow, new ItemBody(GenderDict[Gander][BodyItems.EyeBrow].First(), 0, true));
            Body.Add(BodyItems.Torso, new ItemBody(GenderDict[Gander][BodyItems.Torso].First(), 0, true));
            Body.Add(BodyItems.ArmUpper_Right, new ItemBody(GenderDict[Gander][BodyItems.ArmUpper_Right].First(), 0, true));
            Body.Add(BodyItems.ArmUpper_Left, new ItemBody(GenderDict[Gander][BodyItems.ArmUpper_Left].First(), 0, true));
            Body.Add(BodyItems.ArmLower_Right, new ItemBody(GenderDict[Gander][BodyItems.ArmLower_Right].First(), 0, true));
            Body.Add(BodyItems.ArmLower_Left, new ItemBody(GenderDict[Gander][BodyItems.ArmLower_Left].First(), 0, true));
            Body.Add(BodyItems.Hand_Right, new ItemBody(GenderDict[Gander][BodyItems.Hand_Right].First(), 0, true));
            Body.Add(BodyItems.Hand_Left, new ItemBody(GenderDict[Gander][BodyItems.Hand_Left].First(), 0, true));
            Body.Add(BodyItems.Hips, new ItemBody(GenderDict[Gander][BodyItems.Hips].First(), 0, true));
            Body.Add(BodyItems.Leg_Right, new ItemBody(GenderDict[Gander][BodyItems.Leg_Right].First(), 0, true));
            Body.Add(BodyItems.Leg_Left, new ItemBody(GenderDict[Gander][BodyItems.Leg_Left].First(), 0, true));

            Body.Add(BodyItems.Head_Attachment, new ItemBody(NoGenderDict[BodyItems.Head_Attachment].First(), 0, false));
            Body.Add(BodyItems.Back_Attachment, new ItemBody(NoGenderDict[BodyItems.Back_Attachment].First(), 0, false));
            //Body.Add(BodyItems.Chest_Attachment, new Item(_noGenderDict[BodyItems.Chest_Attachment].First(), 0));
            Body.Add(BodyItems.Elbow_Attachment_Left, new ItemBody(NoGenderDict[BodyItems.Elbow_Attachment_Left].First(), 0, false));
            Body.Add(BodyItems.Elbow_Attachment_Right, new ItemBody(NoGenderDict[BodyItems.Elbow_Attachment_Right].First(), 0, false));
            Body.Add(BodyItems.HeadCoverings_Base_Hair, new ItemBody(NoGenderDict[BodyItems.HeadCoverings_Base_Hair].First(), 0, false));
            Body.Add(BodyItems.HeadCoverings_No_FacialHair, new ItemBody(NoGenderDict[BodyItems.HeadCoverings_No_FacialHair].First(), 0, false));
            Body.Add(BodyItems.HeadCoverings_No_Hair, new ItemBody(NoGenderDict[BodyItems.HeadCoverings_No_Hair].First(), 0, false));
            Body.Add(BodyItems.Hips_Attachment, new ItemBody(NoGenderDict[BodyItems.Hips_Attachment].First(), 0, false));
            Body.Add(BodyItems.Knee_Attachement_Left, new ItemBody(NoGenderDict[BodyItems.Knee_Attachement_Left].First(), 0, false));
            Body.Add(BodyItems.Knee_Attachement_Right, new ItemBody(NoGenderDict[BodyItems.Knee_Attachement_Right].First(), 0, false));
            Body.Add(BodyItems.Shoulder_Attachment_Left, new ItemBody(NoGenderDict[BodyItems.Shoulder_Attachment_Left].First(), 0, false));
            Body.Add(BodyItems.Shoulder_Attachment_Right, new ItemBody(NoGenderDict[BodyItems.Shoulder_Attachment_Right].First(), 0, false));
        }


        public static void Initial(
            CharacterObjectGroups _male,
            CharacterObjectGroups _female,
            CharacterObjectListsAllGender _allGender,
            GameObject _gameObject)
        {
            InitGameObject(_male.headAllElements, "Male_Head_All_Elements", _gameObject);
            InitGameObject(_male.headNoElements, "Male_Head_No_Elements", _gameObject);
            InitGameObject(_male.eyebrow, "Male_01_Eyebrows", _gameObject);
            InitGameObject(_male.facialHair, "Male_02_FacialHair", _gameObject);
            InitGameObject(_male.torso, "Male_03_Torso", _gameObject);
            InitGameObject(_male.arm_Upper_Right, "Male_04_Arm_Upper_Right", _gameObject);
            InitGameObject(_male.arm_Upper_Left, "Male_05_Arm_Upper_Left", _gameObject);
            InitGameObject(_male.arm_Lower_Right, "Male_06_Arm_Lower_Right", _gameObject);
            InitGameObject(_male.arm_Lower_Left, "Male_07_Arm_Lower_Left", _gameObject);
            InitGameObject(_male.hand_Right, "Male_08_Hand_Right", _gameObject);
            InitGameObject(_male.hand_Left, "Male_09_Hand_Left", _gameObject);
            InitGameObject(_male.hips, "Male_10_Hips", _gameObject);
            InitGameObject(_male.leg_Right, "Male_11_Leg_Right", _gameObject);
            InitGameObject(_male.leg_Left, "Male_12_Leg_Left", _gameObject);

            InitGameObject(_female.headAllElements, "Female_Head_All_Elements", _gameObject);
            InitGameObject(_female.headNoElements, "Female_Head_No_Elements", _gameObject);
            InitGameObject(_female.eyebrow, "Female_01_Eyebrows", _gameObject);
            InitGameObject(_female.facialHair, "Female_02_FacialHair", _gameObject);
            InitGameObject(_female.torso, "Female_03_Torso", _gameObject);
            InitGameObject(_female.arm_Upper_Right, "Female_04_Arm_Upper_Right", _gameObject);
            InitGameObject(_female.arm_Upper_Left, "Female_05_Arm_Upper_Left", _gameObject);
            InitGameObject(_female.arm_Lower_Right, "Female_06_Arm_Lower_Right", _gameObject);
            InitGameObject(_female.arm_Lower_Left, "Female_07_Arm_Lower_Left", _gameObject);
            InitGameObject(_female.hand_Right, "Female_08_Hand_Right", _gameObject);
            InitGameObject(_female.hand_Left, "Female_09_Hand_Left", _gameObject);
            InitGameObject(_female.hips, "Female_10_Hips", _gameObject);
            InitGameObject(_female.leg_Right, "Female_11_Leg_Right", _gameObject);
            InitGameObject(_female.leg_Left, "Female_12_Leg_Left", _gameObject);

            InitGameObject(_allGender.all_Hair, "All_01_Hair", _gameObject);
            InitGameObject(_allGender.all_Head_Attachment, "All_02_Head_Attachment", _gameObject);
            InitGameObject(_allGender.headCoverings_Base_Hair, "HeadCoverings_Base_Hair", _gameObject);
            InitGameObject(_allGender.headCoverings_No_FacialHair, "HeadCoverings_No_FacialHair", _gameObject);
            InitGameObject(_allGender.headCoverings_No_Hair, "HeadCoverings_No_Hair", _gameObject);
            InitGameObject(_allGender.chest_Attachment, "All_03_Chest_Attachment", _gameObject);
            InitGameObject(_allGender.back_Attachment, "All_04_Back_Attachment", _gameObject);
            InitGameObject(_allGender.shoulder_Attachment_Right, "All_05_Shoulder_Attachment_Right", _gameObject);
            InitGameObject(_allGender.shoulder_Attachment_Left, "All_06_Shoulder_Attachment_Left", _gameObject);
            InitGameObject(_allGender.elbow_Attachment_Right, "All_07_Elbow_Attachment_Right", _gameObject);
            InitGameObject(_allGender.elbow_Attachment_Left, "All_08_Elbow_Attachment_Left", _gameObject);
            InitGameObject(_allGender.hips_Attachment, "All_09_Hips_Attachment", _gameObject);
            InitGameObject(_allGender.knee_Attachement_Right, "All_10_Knee_Attachement_Right", _gameObject);
            InitGameObject(_allGender.knee_Attachement_Left, "All_11_Knee_Attachement_Left", _gameObject);
            InitGameObject(_allGender.elf_Ear, "Elf_Ear", _gameObject);

            GenderDict = new Dictionary<Gender, Dictionary<BodyItems, List<GameObject>>>
            {
                {Gender.Male,  new Dictionary<BodyItems, List<GameObject>>
                {
                    { BodyItems.ArmLower_Left, _male.arm_Lower_Left },
                    { BodyItems.ArmLower_Right, _male.arm_Lower_Right },
                    { BodyItems.ArmUpper_Left, _male.arm_Upper_Left },
                    { BodyItems.ArmUpper_Right, _male.arm_Upper_Right },
                    { BodyItems.EyeBrow, _male.eyebrow },
                    { BodyItems.FacialHair, _male.facialHair },
                    { BodyItems.Hand_Left, _male.hand_Left },
                    { BodyItems.Hand_Right, _male.hand_Right },
                    { BodyItems.HeadAllElements, _male.headAllElements },
                    { BodyItems.HeadNoElements, _male.headNoElements },
                    { BodyItems.Hips, _male.hips },
                    { BodyItems.Leg_Left, _male.leg_Left },
                    { BodyItems.Leg_Right, _male.leg_Right },
                    { BodyItems.Torso, _male.torso },
                }},

                {Gender.Female,  new Dictionary<BodyItems, List<GameObject>>
                {
                    { BodyItems.ArmLower_Left, _female.arm_Lower_Left },
                    { BodyItems.ArmLower_Right, _female.arm_Lower_Right },
                    { BodyItems.ArmUpper_Left, _female.arm_Upper_Left },
                    { BodyItems.ArmUpper_Right, _female.arm_Upper_Right },
                    { BodyItems.EyeBrow, _female.eyebrow },
                    { BodyItems.FacialHair, _female.facialHair },
                    { BodyItems.Hand_Left, _female.hand_Left },
                    { BodyItems.Hand_Right, _female.hand_Right },
                    { BodyItems.HeadAllElements, _female.headAllElements },
                    { BodyItems.HeadNoElements, _female.headNoElements },
                    { BodyItems.Hips, _female.hips },
                    { BodyItems.Leg_Left, _female.leg_Left },
                    { BodyItems.Leg_Right, _female.leg_Right },
                    { BodyItems.Torso, _female.torso },
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
        }


        private static void InitGameObject(List<GameObject> targetList, string characterPart, GameObject _gameObject)
        {
            Transform[] rootTransform = _gameObject.GetComponentsInChildren<Transform>();
            Transform targetRoot = null;
            foreach (Transform t in rootTransform)
            {
                if (t.gameObject.name == characterPart)
                {
                    targetRoot = t;
                    break;
                }
            }
            targetList.Clear();
            for (int i = 0; i < targetRoot.childCount; i++)
            {
                GameObject go = targetRoot.GetChild(i).gameObject;
                go.SetActive(false);
                targetList.Add(go);
            }
        }



        public List<Tuple<ActForItem, List<ItemBody>>> ReadAllCharacter()
        {
            var list = Body.Select((x, y) => x.Value).ToList();
            var listrRemove = new List<ItemBody>();
            if (!HairToggle)
            {
                list.Remove(Body[BodyItems.Hair]);
                listrRemove.Add(Body[BodyItems.Hair]);
            }

            if (!FacialHairToggle)
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

            LoadColor();

            return new List<Tuple<ActForItem, List<ItemBody>>> { Tuple.Create(ActForItem.Add, Body.Select((x, y) => x.Value).ToList()),
            Tuple.Create(ActForItem.Remove, listrRemove )};
        }

        private void LoadColor()
        {

            Material.SetColor("_Color_Primary", Color.GearPrimary);
            Material.SetColor("_Color_Secondary", Color.GearSecondary);
            Material.SetColor("_Color_Metal_Primary", Color.MetalPrimary);
            Material.SetColor("_Color_Metal_Secondary", Color.MetalSecondary);
            Material.SetColor("_Color_Leather_Primary", Color.LeatherPrimary);
            Material.SetColor("_Color_Leather_Secondary", Color.LeatherSecondary);
            Material.SetColor("_Color_BodyArt", Color.BodyArtColors);
            Material.SetFloat("_BodyArt_Amount", Color.BodyArtAmount);
            Material.SetColor("_Color_Skin", Color.Skin);
            Material.SetColor("_Color_Hair", Color.Hair);
            Material.SetColor("_Color_Stubble", Color.Stubble);
            Material.SetColor("_Color_Scar", Color.Scar);
        }

        public List<Tuple<ActForItem, List<ItemBody>>> ClearAllCharacter()
        {
            var list = Body.Select((x, y) => x.Value).ToList();
            list.ForEach(x => new ItemBody(x.Object, x.Position, false));
            return new List<Tuple<ActForItem, List<ItemBody>>> { Tuple.Create(ActForItem.Remove, list) };
        }

        public int GetPosition(BodyItems body)
        {
            return Body[body].Position;
        }

        public List<Tuple<ActForItem, List<ItemBody>>> ChangeRace(Race _race)
        {
            Race = _race;
            if (_race == Race.Elf)
            {
                Body[BodyItems.Ear].Object = NoGenderDict[BodyItems.Ear][Body[BodyItems.Ear].Position];
                Body[BodyItems.Ear].Visible = true;
                return new List<Tuple<ActForItem, List<ItemBody>>> { Tuple.Create(ActForItem.Add, new List<ItemBody> { Body[BodyItems.Ear] }) };
            }
            else if (_race == Race.Human)
            {
                var ear = Body[BodyItems.Ear].Object;
                Body[BodyItems.Ear].Object = NoGenderDict[BodyItems.Ear][Body[BodyItems.Ear].Position];
                Body[BodyItems.Ear].Visible = false;
                return new List<Tuple<ActForItem, List<ItemBody>>> { Tuple.Create(ActForItem.Remove, new List<ItemBody> { new ItemBody(ear, Body[BodyItems.Ear].Position, false) }) };
            }
            else
            {
                return new List<Tuple<ActForItem, List<ItemBody>>> { Tuple.Create(ActForItem.None, new List<ItemBody> { }) };
            }
        }

        public List<Tuple<ActForItem, List<ItemBody>>> ChangeGender(Gender _gender)
        {
            if (_gender == Gander)
            {
                return new List<Tuple<ActForItem, List<ItemBody>>> { Tuple.Create(ActForItem.None, new List<ItemBody> { }) };
            }

            Gander = _gender;

            var list = new List<Tuple<ActForItem, List<ItemBody>>> { };

            list.Add(Tuple.Create(ActForItem.Remove, new List<ItemBody>
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

            Body[BodyItems.HeadAllElements] = ItemGenderChange(Body[BodyItems.HeadAllElements], _gender, GenderDict[Gender.Male][BodyItems.HeadAllElements], GenderDict[Gender.Female][BodyItems.HeadAllElements]);
            Body[BodyItems.EyeBrow] = ItemGenderChange(Body[BodyItems.EyeBrow], _gender, GenderDict[Gender.Male][BodyItems.EyeBrow], GenderDict[Gender.Female][BodyItems.EyeBrow]);
            Body[BodyItems.FacialHair] = ItemGenderChange(Body[BodyItems.Hair], _gender, GenderDict[Gender.Male][BodyItems.FacialHair], GenderDict[Gender.Female][BodyItems.FacialHair]);
            Body[BodyItems.Torso] = ItemGenderChange(Body[BodyItems.Torso], _gender, GenderDict[Gender.Male][BodyItems.Torso], GenderDict[Gender.Female][BodyItems.Torso]);
            Body[BodyItems.ArmUpper_Right] = ItemGenderChange(Body[BodyItems.ArmUpper_Right], _gender, GenderDict[Gender.Male][BodyItems.ArmUpper_Right], GenderDict[Gender.Female][BodyItems.ArmUpper_Right]);
            Body[BodyItems.ArmUpper_Left] = ItemGenderChange(Body[BodyItems.ArmUpper_Left], _gender, GenderDict[Gender.Male][BodyItems.ArmUpper_Left], GenderDict[Gender.Female][BodyItems.ArmUpper_Left]);
            Body[BodyItems.ArmLower_Right] = ItemGenderChange(Body[BodyItems.ArmLower_Right], _gender, GenderDict[Gender.Male][BodyItems.ArmLower_Right], GenderDict[Gender.Female][BodyItems.ArmLower_Right]);
            Body[BodyItems.ArmLower_Left] = ItemGenderChange(Body[BodyItems.ArmLower_Left], _gender, GenderDict[Gender.Male][BodyItems.ArmLower_Left], GenderDict[Gender.Female][BodyItems.ArmLower_Left]);
            Body[BodyItems.Hand_Right] = ItemGenderChange(Body[BodyItems.Hand_Right], _gender, GenderDict[Gender.Male][BodyItems.Hand_Right], GenderDict[Gender.Female][BodyItems.Hand_Right]);
            Body[BodyItems.Hand_Left] = ItemGenderChange(Body[BodyItems.Hand_Left], _gender, GenderDict[Gender.Male][BodyItems.Hand_Left], GenderDict[Gender.Female][BodyItems.Hand_Left]);
            Body[BodyItems.Hips] = ItemGenderChange(Body[BodyItems.Hips], _gender, GenderDict[Gender.Male][BodyItems.Hips], GenderDict[Gender.Female][BodyItems.Hips]);
            Body[BodyItems.Leg_Right] = ItemGenderChange(Body[BodyItems.Leg_Right], _gender, GenderDict[Gender.Male][BodyItems.Leg_Right], GenderDict[Gender.Female][BodyItems.Leg_Right]);
            Body[BodyItems.Leg_Left] = ItemGenderChange(Body[BodyItems.Leg_Left], _gender, GenderDict[Gender.Male][BodyItems.Hand_Left], GenderDict[Gender.Female][BodyItems.Hand_Left]);

            list.Add(Tuple.Create(ActForItem.Add, new List<ItemBody>
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

        public ItemBody ItemGenderChange(ItemBody _item, Gender _gender, List<GameObject> _list_male, List<GameObject> _list_female)
        {
            if ((_gender is Gender.Male
                ? _list_male.Count
                : _list_female.Count) <= _item.Position || _item.Position < 0)
            {
                _item.Position = (_gender is Gender.Male
                    ? _list_male.Count
                    : _list_female.Count) - 1;
            }

            return new ItemBody(_item.Position < 0
                ? null
                    : _gender is Gender.Male
                ? _list_male[_item.Position]
                    : _list_female[_item.Position], _item.Position,
                    _item.Visible);
        }

        public List<Tuple<ActForItem, List<ItemBody>>> ChangeForIndex(BodyItems body, int index)
        {
            //if (!Body.TryGetValue(body, out var _item) || _item.Position == index)
            //{
            //    return new List<Tuple<ActForItem, List<Item>>> { Tuple.Create(ActForItem.None, new List<Item> { }) };
            //}

            var list = new List<Tuple<ActForItem, List<ItemBody>>> { Tuple.Create(ActForItem.Remove, new List<ItemBody> { new ItemBody(Body[body].Object, Body[body].Position, false) }) };

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

            list.Add(Tuple.Create(ActForItem.Add, new List<ItemBody> { Body[body] }));
            return list;
        }

        public List<Tuple<ActForItem, List<ItemBody>>> ChangeNext(BodyItems body)
        {
            if (!Body.TryGetValue(body, out var _))
            {
                return new List<Tuple<ActForItem, List<ItemBody>>> { Tuple.Create(ActForItem.None, new List<ItemBody> { }) };
            }

            var list = new List<Tuple<ActForItem, List<ItemBody>>> { Tuple.Create(ActForItem.Remove, new List<ItemBody> { new ItemBody(Body[body].Object, Body[body].Position, false) }) };

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

            list.Add(Tuple.Create(ActForItem.Add, new List<ItemBody> { Body[body] }));
            return list;
        }

        public List<Tuple<ActForItem, List<ItemBody>>> ChangePrevious(BodyItems body)
        {
            if (!Body.TryGetValue(body, out var _item))
            {
                return new List<Tuple<ActForItem, List<ItemBody>>> { Tuple.Create(ActForItem.None, new List<ItemBody> { }) };
            }

            var list = new List<Tuple<ActForItem, List<ItemBody>>> { Tuple.Create(ActForItem.Remove, new List<ItemBody> { new ItemBody(Body[body].Object, Body[body].Position, false) }) };

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

            list.Add(Tuple.Create(ActForItem.Add, new List<ItemBody> { Body[body] }));
            return list;
        }

        public List<Tuple<ActForItem, List<ItemBody>>> ChangeDisable(BodyItems body)
        {
            if (!Body.TryGetValue(body, out var _))
            {
                return new List<Tuple<ActForItem, List<ItemBody>>> { Tuple.Create(ActForItem.None, new List<ItemBody> { }) };
            }

            return new List<Tuple<ActForItem, List<ItemBody>>> { Tuple.Create(ActForItem.Remove, new List<ItemBody> { new ItemBody(Body[body].Object, Body[body].Position, false) }) };
        }

        public List<Tuple<ActForItem, List<ItemBody>>> ChangeEnable(BodyItems body)
        {
            if (!Body.TryGetValue(body, out var _))
            {
                return new List<Tuple<ActForItem, List<ItemBody>>> { Tuple.Create(ActForItem.None, new List<ItemBody> { }) };
            }

            return new List<Tuple<ActForItem, List<ItemBody>>> { Tuple.Create(ActForItem.Add, new List<ItemBody> { new ItemBody(Body[body].Object, Body[body].Position, true) }) };
        }

        internal void Load(CharacterSave _loadDate)
        {
            if (_loadDate.Toggle.Count >= 14)
            {
                HairToggle = _loadDate.Toggle[0];
                FacialHairToggle = _loadDate.Toggle[1];
                HeadAllElementsToggle = _loadDate.Toggle[2];
                HeadCov1Toggle = _loadDate.Toggle[3];
                HeadCov2Toggle = _loadDate.Toggle[4];
                HeadCov3Toggle = _loadDate.Toggle[5];
                ChestToggle = _loadDate.Toggle[6];
                SholderRToggle = _loadDate.Toggle[7];
                SholderLToggle = _loadDate.Toggle[8];
                ElbowRToggle = _loadDate.Toggle[9];
                ElbowLToggle = _loadDate.Toggle[10];
                KneeRToggle = _loadDate.Toggle[11];
                KneeLToggle = _loadDate.Toggle[12];
                HipsAToggle = _loadDate.Toggle[13];
            }

            Gander = _loadDate.Gender;
            Race = _loadDate.Race;
            Color = _loadDate.Color;

            foreach (var item in _loadDate.Items)
            {
                if ((GenderHash.Contains(item.Body) ? GenderDict[_loadDate.Gender][item.Body].Count : NoGenderDict[item.Body].Count) > item.Position)
                {
                    Body[item.Body].Object = GenderHash.Contains(item.Body) ? GenderDict[_loadDate.Gender][item.Body][item.Position] : NoGenderDict[item.Body][item.Position];
                    Body[item.Body].Position = item.Position;
                    Body[item.Body].Visible = item.Visible;
                }
            }
        }

        #endregion
    }

    [Serializable]
    public class ColorationSkin
    {
        public Color Skin;
        public Color Hair;
        public Color Stubble;
        public Color Scar;
        public Color GearPrimary;
        public Color GearSecondary;
        public Color MetalPrimary;
        public Color MetalSecondary;
        public Color LeatherPrimary;
        public Color LeatherSecondary;
        public Color BodyArtColors;
        public float BodyArtAmount;

        public ColorationSkin() { }

        public ColorationSkin(Color _skin, Color _hair, Color _stubble, Color _scar, Color _gearPrimary, Color _gearSecondary, Color _metalPrimary, Color _metalSecondary, Color _leatherPrimary, Color _leatherSecondary, Color _bodyArtColors, float _artArtAmount)
        {
            Skin = _skin;
            Hair = _hair;
            Stubble = _stubble;
            Scar = _scar;
            GearPrimary = _gearPrimary;
            GearSecondary = _gearSecondary;
            MetalPrimary = _metalPrimary;
            MetalSecondary = _metalSecondary;
            LeatherPrimary = _leatherPrimary;
            LeatherSecondary = _leatherSecondary;
            BodyArtColors = _bodyArtColors;
            BodyArtAmount = _artArtAmount;
        }
    }
}
