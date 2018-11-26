using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveDados : MonoBehaviour
{
    public static SaveDados instance;

    [SerializeField]
    private InputField inputNamePlayer, inputSenha;

    [SerializeField]
    private Text txt_informativo;

    public string NamePlayerCarregado;


    private void Awake()
    {
        instance = this;
    }

    public void Savar()
    {
        if (File.Exists(Application.persistentDataPath + inputNamePlayer.text))
        {
            txt_informativo.text = "this user is not available";
        }
        else if (!File.Exists(Application.persistentDataPath + inputNamePlayer.text))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Create(Application.persistentDataPath + inputNamePlayer.text);
            FileStream fs1 = File.Create(Application.persistentDataPath + inputSenha.text);

            SalvaDados dados = new SalvaDados();
            dados.Name = inputNamePlayer.text;
            dados.Senha = inputSenha.text;
            bf.Serialize(fs, dados);
            fs.Close();
            txt_informativo.text = "Created successfully!";
            inputNamePlayer.text = "";
        }
    }
    

    public void DeletaConta()
    {
        if(File.Exists(Application.persistentDataPath + inputNamePlayer.text))
        {
            if (File.Exists(Application.persistentDataPath + inputSenha.text))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = File.Open(Application.persistentDataPath + inputNamePlayer.text, FileMode.Open);
                FileStream fs1 = File.Open(Application.persistentDataPath + inputSenha.text, FileMode.Open);
                SalvaDados dados = (SalvaDados)bf.Deserialize(fs);

                File.Delete(dados.Name);
                File.Delete(dados.Senha);

                txt_informativo.text = "Account successfully deleted!";
            }
            else
            {
                txt_informativo.text = "Wrong Password, could not be deleted !";
            }
        }
        else
        {
            txt_informativo.text = "Does not exist!";
        }
    }

    public void LoadDados()
    {
        if(File.Exists(Application.persistentDataPath + inputNamePlayer.text))
        {
            if(File.Exists(Application.persistentDataPath + inputSenha.text))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = File.Open(Application.persistentDataPath + inputNamePlayer.text, FileMode.Open);
                FileStream fs1 = File.Open(Application.persistentDataPath + inputSenha.text, FileMode.Open);

                SalvaDados dados = (SalvaDados)bf.Deserialize(fs);
                fs.Close();

                NamePlayerCarregado = dados.Name;
                SceneManager.LoadScene(1);
            }
            else
            {
                txt_informativo.text = "Wrong Password !";
            }
        }
        else
        {
            txt_informativo.text = "Name or Password does not exist, check that they are correct.";
        }
    }
}

[Serializable]
class SalvaDados
{
   public string Name;
   public string Senha;
}