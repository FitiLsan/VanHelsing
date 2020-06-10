using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BeastHunter
{
    public class CharacterFormerController : MonoBehaviour
    {
        #region Field

        [Header("Gear Colors")]
        public Color[] primary = { new Color(0.2862745f, 0.4f, 0.4941177f), new Color(0.4392157f, 0.1960784f, 0.172549f), new Color(0.3529412f, 0.3803922f, 0.2705882f), new Color(0.682353f, 0.4392157f, 0.2196079f), new Color(0.4313726f, 0.2313726f, 0.2705882f), new Color(0.5921569f, 0.4941177f, 0.2588235f), new Color(0.482353f, 0.4156863f, 0.3529412f), new Color(0.2352941f, 0.2352941f, 0.2352941f), new Color(0.2313726f, 0.4313726f, 0.4156863f) };
        public Color[] secondary = { new Color(0.7019608f, 0.6235294f, 0.4666667f), new Color(0.7372549f, 0.7372549f, 0.7372549f), new Color(0.1647059f, 0.1647059f, 0.1647059f), new Color(0.2392157f, 0.2509804f, 0.1882353f) };

        [Header("Metal Colors")]
        public Color[] metalPrimary = { new Color(0.6705883f, 0.6705883f, 0.6705883f), new Color(0.5568628f, 0.5960785f, 0.6392157f), new Color(0.5568628f, 0.6235294f, 0.6f), new Color(0.6313726f, 0.6196079f, 0.5568628f), new Color(0.6980392f, 0.6509804f, 0.6196079f) };
        public Color[] metalSecondary = { new Color(0.3921569f, 0.4039216f, 0.4117647f), new Color(0.4784314f, 0.5176471f, 0.5450981f), new Color(0.3764706f, 0.3607843f, 0.3372549f), new Color(0.3254902f, 0.3764706f, 0.3372549f), new Color(0.4f, 0.4039216f, 0.3568628f) };

        [Header("Leather Colors")]
        public Color[] leatherPrimary;
        public Color[] leatherSecondary;

        [Header("Material")]
        public Material mat;

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


        private bool Randomizer = false;
        public Stack<CharacterPresentationModel> MemoryCharacters = new Stack<CharacterPresentationModel>();
        public Stack<CharacterPresentationModel> PrevCharacters = new Stack<CharacterPresentationModel>();
        public Stack<CharacterPresentationModel> NextCharacters = new Stack<CharacterPresentationModel>();
        public CharacterPresentationModel Character;

        public float shuffleSpeed = 0.7f;

        public string NameFile;

        [HideInInspector]
        public CharacterObjectGroups male;
        
        [HideInInspector]
        public CharacterObjectGroups female;

        [HideInInspector]
        public CharacterObjectListsAllGender allGender;

        SkinColor _skinColor = SkinColor.White;
        Elements _elements = Elements.Yes;
        BodyItems _headCovering = BodyItems.HeadCoverings_Base_Hair;
        FacialHair _facialHair = FacialHair.Yes;

        #endregion


        #region FieldCam

        Transform camHolder;

        float _x = 16;

        float _y = -30;

        #endregion


        #region Unity

        void Start()
        {
            CharacterPresentationModel.Initial(male, female, allGender, gameObject);
            Character = new CharacterPresentationModel(new ColorationSkin(
                whiteSkin.FirstOrDefault(),  whiteHair.FirstOrDefault(), whiteStubble, whiteScar, 
                primary.FirstOrDefault(), secondary.FirstOrDefault(), metalPrimary.FirstOrDefault(), metalSecondary.FirstOrDefault(),
                leatherPrimary.FirstOrDefault(), leatherSecondary.FirstOrDefault(), bodyArt.FirstOrDefault(), 0), mat);
            ActivateItem(Character.ReadAllCharacter());

            Transform _cam = Camera.main.transform;
            if (_cam)
            {
                _cam.position = transform.position + new Vector3(0, 0.3f, 2);
                _cam.rotation = Quaternion.Euler(0, -180, 0);
                camHolder = new GameObject().transform;
                camHolder.position = transform.position + new Vector3(0, 1, 0);
                _cam.LookAt(camHolder);
                _cam.SetParent(camHolder);
            }
            
            InvokeRepeating("RandomToggle", shuffleSpeed, shuffleSpeed);
        }

        void Update()
        {
            if (camHolder)
            {
                if (Input.GetKey(KeyCode.Mouse1))
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    _x += 1 * Input.GetAxis("Mouse X");
                    _y -= 1 * Input.GetAxis("Mouse Y");
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    _x -= 1 * Input.GetAxis("Horizontal");
                    _y -= 1 * Input.GetAxis("Vertical");
                }
            }
        }

        void LateUpdate()
        {
            if (camHolder)
            {
                _y = Mathf.Clamp(_y, -45, 15);
                camHolder.eulerAngles = new Vector3(_y, _x, 0.0f);
            }
        }

        #endregion


        #region Method

        public void RandomToggle()
        {

            if (Randomizer)
            {
                RandomChange();
            }
        }

        public void RandomChange()
        {
            PrevCharacters.Push(Character);
            ActivateItem(Character.ClearAllCharacter());
            Character = new CharacterPresentationModel(new ColorationSkin(
            whiteSkin.FirstOrDefault(), whiteHair.FirstOrDefault(), whiteStubble, whiteScar,
            primary.FirstOrDefault(), secondary.FirstOrDefault(), metalPrimary.FirstOrDefault(), metalSecondary.FirstOrDefault(),
            leatherPrimary.FirstOrDefault(), leatherSecondary.FirstOrDefault(), bodyArt.FirstOrDefault(), 0), mat);

            if (!Rand(50))
                ActivateItem(Character.ChangeGender(Gender.Female));

            if (!Rand(40))
            {
                ElfToggle(true);
                ActivateItem(Character.ChangeRace(Race.Elf));
            }
            else
            {
                ElfToggle(false);
            }

            if (!Rand(60))
                _elements = Elements.No;

            if (Rand(33))
                _headCovering = BodyItems.HeadCoverings_Base_Hair;
            else if (Rand(33))
                _headCovering = BodyItems.HeadCoverings_No_FacialHair;
            else
                _headCovering = BodyItems.HeadCoverings_No_Hair;

            switch (Character.Race)
            {
                case Race.Human:
                    if (Rand(33))
                    {
                        Debug.Log("White");
                        Character.SkinColor = SkinColor.White;
                    }
                    else if (Rand(33))
                    {
                        Debug.Log("Brown");
                        Character.SkinColor = SkinColor.Brown;
                    }
                    else
                    {
                        Debug.Log("Black");
                        Character.SkinColor = SkinColor.Black;
                    }
                    break;
                case Race.Elf:
                    Debug.Log("Elf");
                    Character.SkinColor = SkinColor.Elf;
                    break;
            }

            LoadColors();

            if (!Rand(40))
                _facialHair = FacialHair.No;

            Character.HairToggle = false;
            Character.FacialHairToggle = false;

            switch (_elements)
            {
                case Elements.Yes:
                    if (CharacterPresentationModel.GenderDict[Character.Gander][BodyItems.HeadAllElements].Count != 0)
                        ActivateItem(Character.ChangeForIndex(BodyItems.HeadAllElements, Random.Range(0, CharacterPresentationModel.GenderDict[Character.Gander][BodyItems.HeadAllElements].Count)));

                    if (CharacterPresentationModel.GenderDict[Character.Gander][BodyItems.EyeBrow].Count != 0)
                        ActivateItem(Character.ChangeForIndex(BodyItems.EyeBrow, Random.Range(0, CharacterPresentationModel.GenderDict[Character.Gander][BodyItems.EyeBrow].Count)));

                    if (CharacterPresentationModel.GenderDict[Character.Gander][BodyItems.FacialHair].Count != 0 && _facialHair == FacialHair.Yes && Character.Gander == Gender.Male && _headCovering != BodyItems.HeadCoverings_No_FacialHair)
                    {
                        ActivateItem(Character.ChangeForIndex(BodyItems.FacialHair, Random.Range(0, CharacterPresentationModel.GenderDict[Character.Gander][BodyItems.FacialHair].Count)));
                        Character.FacialHairToggle = true;
                    }

                    switch (_headCovering)
                    {
                        case BodyItems.HeadCoverings_Base_Hair:
                            if (CharacterPresentationModel.NoGenderDict[BodyItems.Hair].Count != 0)
                                ActivateItem(Character.ChangeForIndex(BodyItems.Hair, Random.Range(0, CharacterPresentationModel.NoGenderDict[BodyItems.Hair].Count)));
                            if (CharacterPresentationModel.NoGenderDict[BodyItems.HeadCoverings_Base_Hair].Count != 0)
                                ActivateItem(Character.ChangeForIndex(BodyItems.HeadCoverings_Base_Hair, Random.Range(0, CharacterPresentationModel.NoGenderDict[BodyItems.HeadCoverings_Base_Hair].Count)));
                            Character.HairToggle = true;
                            break;
                        case BodyItems.HeadCoverings_No_FacialHair:
                            if (CharacterPresentationModel.NoGenderDict[BodyItems.Hair].Count != 0)
                                ActivateItem(Character.ChangeForIndex(BodyItems.Hair, Random.Range(0, CharacterPresentationModel.NoGenderDict[BodyItems.Hair].Count)));
                            if (CharacterPresentationModel.NoGenderDict[BodyItems.HeadCoverings_No_FacialHair].Count != 0)
                                ActivateItem(Character.ChangeForIndex(BodyItems.HeadCoverings_No_FacialHair, Random.Range(0, CharacterPresentationModel.NoGenderDict[BodyItems.HeadCoverings_No_FacialHair].Count)));
                            Character.HairToggle = true;
                            break;
                        case BodyItems.HeadCoverings_No_Hair:
                            if (CharacterPresentationModel.NoGenderDict[BodyItems.HeadCoverings_No_Hair].Count != 0)
                                ActivateItem(Character.ChangeForIndex(BodyItems.HeadCoverings_No_Hair, Random.Range(0, CharacterPresentationModel.NoGenderDict[BodyItems.HeadCoverings_No_Hair].Count)));
                            if (Character.Race != Race.Human)
                            {
                                if (CharacterPresentationModel.NoGenderDict[BodyItems.Ear].Count != 0)
                                    ActivateItem(Character.ChangeForIndex(BodyItems.Ear, Random.Range(0, CharacterPresentationModel.NoGenderDict[BodyItems.Ear].Count)));
                            }
                            break;
                    }
                    break;

                case Elements.No:
                    if (CharacterPresentationModel.GenderDict[Character.Gander][BodyItems.HeadNoElements].Count != 0)
                        ActivateItem(Character.ChangeForIndex(BodyItems.HeadNoElements, Random.Range(0, CharacterPresentationModel.GenderDict[Character.Gander][BodyItems.HeadNoElements].Count)));
                    break;
            }
            if (CharacterPresentationModel.GenderDict[Character.Gander][BodyItems.ArmUpper_Right].Count != 0)
                GetLeftRight(BodyItems.ArmUpper_Right, BodyItems.ArmUpper_Left, 20);

            if (CharacterPresentationModel.GenderDict[Character.Gander][BodyItems.ArmLower_Right].Count != 0)
                GetLeftRight(BodyItems.ArmLower_Right, BodyItems.ArmLower_Left, 20);

            if (CharacterPresentationModel.NoGenderDict[BodyItems.Chest_Attachment].Count != 0)
                ActivateItem(Character.ChangeForIndex(BodyItems.Chest_Attachment, Random.Range(0, CharacterPresentationModel.NoGenderDict[BodyItems.Chest_Attachment].Count)));

            if (CharacterPresentationModel.NoGenderDict[BodyItems.Back_Attachment].Count != 0)
                ActivateItem(Character.ChangeForIndex(BodyItems.Back_Attachment, Random.Range(0, CharacterPresentationModel.NoGenderDict[BodyItems.Back_Attachment].Count)));

            if (CharacterPresentationModel.GenderDict[Character.Gander][BodyItems.Hand_Right].Count != 0)
                GetLeftRight(BodyItems.Hand_Right, BodyItems.Hand_Left, 20);

            if (CharacterPresentationModel.GenderDict[Character.Gander][BodyItems.Hips].Count != 0)
                ActivateItem(Character.ChangeForIndex(BodyItems.Hips, Random.Range(1, CharacterPresentationModel.GenderDict[Character.Gander][BodyItems.Hips].Count)));

            if (CharacterPresentationModel.GenderDict[Character.Gander][BodyItems.Torso].Count != 0)
                ActivateItem(Character.ChangeForIndex(BodyItems.Torso, Random.Range(1, CharacterPresentationModel.GenderDict[Character.Gander][BodyItems.Torso].Count)));

            if (CharacterPresentationModel.GenderDict[Character.Gander][BodyItems.Leg_Right].Count != 0)
                GetLeftRight(BodyItems.Leg_Right, BodyItems.Leg_Left, 20);

            if (CharacterPresentationModel.NoGenderDict[BodyItems.Shoulder_Attachment_Right].Count != 0)
                GetLeftRight(BodyItems.Shoulder_Attachment_Right, BodyItems.Shoulder_Attachment_Left, 15);

            if (CharacterPresentationModel.NoGenderDict[BodyItems.Elbow_Attachment_Right].Count != 0)
                GetLeftRight(BodyItems.Elbow_Attachment_Right, BodyItems.Elbow_Attachment_Left, 15);

            if (CharacterPresentationModel.NoGenderDict[BodyItems.Knee_Attachement_Right].Count != 0)
                GetLeftRight(BodyItems.Knee_Attachement_Right, BodyItems.Knee_Attachement_Left, 15);

            if (CharacterPresentationModel.NoGenderDict[BodyItems.Hips_Attachment].Count != 0)
                ActivateItem(Character.ChangeForIndex(BodyItems.Hips_Attachment, Random.Range(0, CharacterPresentationModel.NoGenderDict[BodyItems.Hips_Attachment].Count)));
        }

        void LoadColors()
        {
            switch (Character.SkinColor)
            {
                case SkinColor.White:
                    RandomizeAndSetHairSkinColors("White", whiteSkin, whiteHair, whiteStubble, whiteScar);
                    break;

                case SkinColor.Brown:
                    RandomizeAndSetHairSkinColors("Brown", brownSkin, brownHair, brownStubble, brownScar);
                    break;

                case SkinColor.Black:
                    RandomizeAndSetHairSkinColors("Black", blackSkin, blackHair, blackStubble, blackScar);
                    break;

                case SkinColor.Elf:
                    RandomizeAndSetHairSkinColors("Elf", elfSkin, elfHair, elfStubble, elfScar);
                    break;
            }

            if (primary.Length != 0)
            {
                Character.Color.GearPrimary = primary[Random.Range(0, primary.Length)];
                mat.SetColor("_Color_Primary", Character.Color.GearPrimary);
            }

            if (secondary.Length != 0)
            {
                Character.Color.GearSecondary = secondary[Random.Range(0, secondary.Length)];
                mat.SetColor("_Color_Secondary", Character.Color.GearSecondary);
            }

            if (metalPrimary.Length != 0)
            {
                Character.Color.MetalPrimary = metalPrimary[Random.Range(0, metalPrimary.Length)];
                mat.SetColor("_Color_Metal_Primary", Character.Color.MetalPrimary);
            }

            if (metalSecondary.Length != 0)
            {
                Character.Color.MetalSecondary = metalSecondary[Random.Range(0, metalSecondary.Length)];
                mat.SetColor("_Color_Metal_Secondary", Character.Color.MetalSecondary);
            }

            if (leatherPrimary.Length != 0)
            {
                Character.Color.LeatherPrimary = leatherPrimary[Random.Range(0, leatherPrimary.Length)];
                mat.SetColor("_Color_Leather_Primary", Character.Color.LeatherPrimary);
            }

            if (leatherSecondary.Length != 0)
            {
                Character.Color.LeatherSecondary = leatherSecondary[Random.Range(0, leatherSecondary.Length)];
                mat.SetColor("_Color_Leather_Secondary", Character.Color.LeatherSecondary);
            }

            if (bodyArt.Length != 0)
            {
                Character.Color.BodyArtColors = bodyArt[Random.Range(0, bodyArt.Length)];
                mat.SetColor("_Color_BodyArt", Character.Color.BodyArtColors);
            }

            mat.SetFloat("_BodyArt_Amount", Random.Range(0.0f, 1.0f));
        }


        void RandomizeAndSetHairSkinColors(string info, Color[] skin, Color[] hair, Color stubble, Color scar)
        {
            if (skin.Length != 0)
            {
                Character.Color.Skin = hair[Random.Range(0, skin.Length)];
                mat.SetColor("_Color_Skin", Character.Color.Skin);
            }

            if (hair.Length != 0)
            {
                Character.Color.Hair = hair[Random.Range(0, hair.Length)];
                mat.SetColor("_Color_Hair", Character.Color.Hair);
            }
            Character.Color.Stubble = stubble;
            Character.Color.Scar = scar;
            mat.SetColor("_Color_Stubble", Character.Color.Stubble);
            mat.SetColor("_Color_Scar", Character.Color.Scar);
        }

        void GetLeftRight(BodyItems objectRight, BodyItems objectLeft, int rndPercent)
        {
            int index = Random.Range(0, Character.GenderHash.Contains(objectRight)
                        ? CharacterPresentationModel.GenderDict[Character.Gander][objectRight].Count
                        : CharacterPresentationModel.NoGenderDict[objectRight].Count);
            ActivateItem(Character.ChangeForIndex(objectRight, index));
            if (Rand(rndPercent))
                index = Random.Range(0, Character.GenderHash.Contains(objectLeft)
                                ? CharacterPresentationModel.GenderDict[Character.Gander][objectLeft].Count
                                : CharacterPresentationModel.NoGenderDict[objectLeft].Count);
            ActivateItem(Character.ChangeForIndex(objectLeft, index));
        }

        bool Rand(int num) => Random.Range(0, 100) <= num;        

        void ActivateItem(List<Tuple<ActForItem, List<ItemBody>>> _objList)
        {
            foreach (var _act in _objList)
            {
                if (_act.Item1 == ActForItem.Add)
                {
                    foreach (ItemBody _item in _act.Item2)
                    {
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
                    foreach (ItemBody _item in _act.Item2)
                    {
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

        #endregion


        #region MathodUIElements


        public void SaveFile()
        {
            SaveLoadDialog.SaveCharacter(
                new CharacterSave(
                    new List<bool> { Character.HairToggle, Character.FacialHairToggle, Character.HeadAllElementsToggle, Character.HeadCov1Toggle, Character.HeadCov2Toggle, Character.HeadCov3Toggle, Character.ChestToggle, Character.SholderRToggle, Character.SholderLToggle, Character.ElbowRToggle, Character.ElbowLToggle, Character.KneeRToggle, Character.KneeLToggle, Character.HipsAToggle },
                    Character.Body.Select((x, y) => new SaveItemBody(x.Key, new ItemBody(null, x.Value.Position, x.Value.Visible))).ToList(),
                    Character.Gander,
                    Character.Race,
                    Character.Color),
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
            Character = new CharacterPresentationModel(new ColorationSkin(
                whiteSkin.FirstOrDefault(), whiteHair.FirstOrDefault(), whiteStubble, whiteScar,
                primary.FirstOrDefault(), secondary.FirstOrDefault(), metalPrimary.FirstOrDefault(), metalSecondary.FirstOrDefault(),
                leatherPrimary.FirstOrDefault(), leatherSecondary.FirstOrDefault(), bodyArt.FirstOrDefault(), 0), mat);
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

        #endregion
    }

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
