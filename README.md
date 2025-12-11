# [NOME DO PROJETO]												   
## Architecture                                                       
- [ ] Dotnet/Aspnet Core 3.1                                          
- [ ] AWS Secrets Manager                                             
- [ ] Swagger                                                         
- [ ] Sentry                                                          
- [ ] Logstash                                                        
- [ ] ElasticSearch                                                   
- [ ] Docker                                                          
- [ ] Docker-Compose                                                  
- [ ] JTW Autorization                                                
## Before Build                                                       
#### Download and Install .NetCore and AspNetCore                     
- https://docs.microsoft.com/pt-br/dotnet/core/install/macos          
- https://docs.microsoft.com/pt-br/dotnet/core/install/linux-ubuntu   
#### Install the version                                              
- dotnet-sdk-3.1                                                      
- aspnetcore-runtime-3.1                                              
## Clone Repository                                                   
```                                                                   
git clone [ENDEREÇO DO REPOSITÓRIO]                                   
```                                                                   
#### Navigate to root project folder.                                 
```                                                                   
cd [NOME DO REPOSITÓRIO]                                              
```                                                                   
## How to Build                                                       
#### Local                                                            
```                                                                   
```                                                                   
#### Docker                                                           
```                                                                   
```                                                                   
## Before Run Instructions                                            
```                                                                   
# AWS CREDENTIALS                                                     
AWS_ACCESS_KEY_ID=                                                    
AWS_SECRET_ACCESS_KEY=                                                
AWS_REGION=                                                           
```                                                                   
## How to Run                                                         
#### Local                                                            
```                                                                   
```                                                                   
#### Docker                                                           
```                                                                   
```                                                                   
#### Application Root API                                             
- http://localhost:5000                                               
#### Swagger                                                          
- http://localhost:5000/swagger                                       
** Padrão CI/CD **                                                    
- Branch `feature/*`: Utilize para implementar novas funcionalidades. Apos conclusao solicite pull request para a branch develop. 
- Branch `develop`: Deploy automatico no ambiente de desenvolvimento.           
- Branch `release/v*.*.*`: Deploy no ambiente de homologacao via solicitacao.    
- Branch `bugfix/`: Utilize para implementar correcoes emergenciais de producao. 
- Branch `master`: Contem a versao atualmente em producao.                      
