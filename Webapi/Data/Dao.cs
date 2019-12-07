using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Webapi.Entities;
using Webapi.Exceptions;

namespace Webapi.Data
{
    public class Dao<T> where T : Entity
    {
        private static string producao = $"Data/Database/";
        private static string teste = $"../../../../Webapi/Data/Database/";
        public static string Caminho = teste;
        public Dao()
        {
            this.Fonte = $"{Caminho}{typeof(T).Name}.json";
        }
        private string Fonte { get; set; } 

        private void EscreverNoArquivo(object objeto)
        {
            try{
                string serializados = JsonConvert.SerializeObject(objeto, Formatting.Indented);  
                File.WriteAllText(this.Fonte, serializados);
            }catch(DirectoryNotFoundException e){

                throw new DAOException($"Caminho do arquivo {typeof(T).Name} não encontrado");
            } 
        }

        public int Adicionar(T item)  
        {   
                var todos = ObterTodos();  

                if(item != null && todos.Any())
                {                   
                    item.Id = todos.Select(i => i.Id).Max() + 1;                    
                }else if(!todos.Any()){
                    item.Id = 1;
                }
                todos.Add(item);

                EscreverNoArquivo(todos);  

                return item.Id;          
        }  

         public int Atualizar(T item)  
        {   
                var todos = ObterTodos();  
                int count = todos.Count;
                if(todos.Any())
                {                   
                    for(int i = 0; i < count; i++)
                    {
                        if(todos.ElementAt(i).Id == item.Id)
                        {
                            todos.RemoveAt(i);
                            todos.Add(item);
                        }
                    }                    
                }

                EscreverNoArquivo(todos);   

                return item.Id;          
        } 
  
        public T BuscarPorId(int id)
        {
            var todos = ObterTodos();

            for(int i = 0; i < todos.Count; i++)
            {
                if(todos.ElementAt(i).Id == id)
                {
                    return todos.ElementAt(i);
                }
            }
            return null;
        }

        public List<T> BuscarPorIds(IEnumerable<int> ids)
        {
            var todos = ObterTodos();

            return todos.Where(o => ids.Contains(o.Id)).ToList();
        }

        public void RemoverPorId(int id)
        {
             var todos = ObterTodos();

            for(int i = 0; i < todos.Count; i++)
            {
                if(todos.ElementAt(i).Id == id)
                {
                     todos.RemoveAt(i);
                     break;
                }
            }
            
            EscreverNoArquivo(todos); 
        }
        public void RemoverTodos()
        {             
            EscreverNoArquivo(new List<T>()); 
        }
        public List<T> ObterTodos()  
        {  
            var json = File.ReadAllText(this.Fonte);  
            var list = json == null || json == string.Empty ? new List<T>() : JsonConvert.DeserializeObject<List<T>>(json);

            return list;
        }
    }  
} 
