# Docker Desktop und CouchDb installieren #
In unsersem Beispiel nutzen wir einen Dockercontainer für die CouchDb installation. <br>
Anschliessened eine kurze Anleitung zur Installation und Ersteinrichtung.
<br>
<br>

# Installation von Docker #
Docker kann unter folgendem Link heruntergeladen werden:
- [Docker für Desktop](https://www.docker.com/get-started) <br>

Eventuell muss muss noch das Linux-Kernel Update für WSL installiert werden:
- [Linux-Kernel Update](https://docs.microsoft.com/de-ch/windows/wsl/install-manual#step-4---download-the-linux-kernel-update-package)
<br>

# Container für CouchDb herunterladen und starten #
Für die fogenden Befehle die Windows PowerShell als Administrator öffnen.

Kontainer herunterladen:
```powershell
PS> docker pull couchdb
```
<br>

Kontainer das erste mal starten:
- bei den Parametern **COUCHDB_USER** und **COUCHDB_PASSWORD** gewünschten User und Passwort eingeben.
```powershell
PS> docker run -p 5984:5984 -d --name my-couchdb -e COUCHDB_USER=admin -e COUCHDB_PASSWORD=admin couchdb:latest
```
<br>

Beim nächsten start kann der Kontainer auch einfach über die Dockerapplikation selbst gestartet werden. Die Parameter werden nicht mehr benötigt.
<br>
<br>

# Ersteinrichtung #
Wenn der Container gestartet ist, kann mit einem beliebigen Browser über *http://localhost:5984/* auf die Datenbank zugegriffen werden. Die Ausgabe sollte etwa wie folgt aussehen:

```json
{
    "couchdb":"Welcome",
    "version":"3.2.0",
    "git_sha":"efb409bba",
    "uuid":"23b60af589892cbfe8bf6541615af7ba",
    "features":["access-ready","partitioned","pluggable-storage-engines","reshard","scheduler"],
    "vendor":
    {
        "name":"The Apache Software Foundation"
    }
}
```
<br>

Über *http://localhost:5984/_utils/* kann auf die Administratorkonsole der Datenbank zugegriffen werden.
- Login mit User und Passwort, die beim ersten start des Kontainers in der Powershell angegeben wurden.
- Über Create Database eine neue DB mit dem Namen testdb anlegen (wird für das Demoprojekt benötigt).
