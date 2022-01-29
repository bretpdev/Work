#!/bin/bash
echo $1
repo_name=`echo $1 | cut -d / -f7`
echo "repo_name: $repo_name"
cd $1

echo "cd $1"
git init
echo "git init"
git add .
echo "git add ."
git commit -m 'Initial commit. (Migration from SVN)'
echo "git commit -m 'Initial commit. (Migration from SVN)'"
git remote add origin git@github.uheaa.ushe.local:UHEAA/$repo_name.git
echo "git remote add origin git@github.uheaa.ushe.local:UHEAA/$repo_name.git"

echo "Creating repo on github"
echo "curl -k -u ebarnes:Update12 -X POST -d '{"name": `echo "$repo_name"`,"homepage": "https://github.uheaa.ushe.local","private": true,"has_issues": true,"has_wiki": false,"has_downloads": true}' https://github.uheaa.ushe.local/api/v3/orgs/UHEAA/repos"
curl -k -u ebarnes:Update12 -X POST -d '{"name": "'"$repo_name"'","homepage": "https://github.uheaa.ushe.local","private": true,"has_issues": true,"has_wiki": false,"has_downloads": true}' https://github.uheaa.ushe.local/api/v3/orgs/UHEAA/repos
echo "git push origin master"
git push origin master


