fx_version 'bodacious'
game 'gta5'

file 'Client/bin/Release/**/publish/*.dll'

client_script 'Client/bin/Release/**/publish/*.net.dll'
server_script 'Server/bin/Release/**/publish/*.net.dll'

ui_page 'UI/index.html'

files {
    'UI/*.html',
    'UI/*.css',
    'UI/*.js'
}

author 'You'
version '1.0.0'
description 'Example Resource from C# Template'