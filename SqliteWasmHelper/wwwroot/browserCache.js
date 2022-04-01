export async function synchronizeDbWithCache(file) {

    window.sqlitedb = window.sqlitedb || {
        init: false,
        cache: await caches.open('SqliteWasmHelper')
    };

    const db = window.sqlitedb;
    
    const backupPath = `/${file}`;
    const cachePath = `/data/cache${backupPath.split('.')[0]}.db`;

    if (!db.init) {

        db.init = true;

        const resp = await db.cache.match(cachePath);

        if (resp && resp.ok) {

            const res = await resp.arrayBuffer();

            if (res) {
                console.log(`Restoring ${res.byteLength} bytes.`);
                FS.writeFile(backupPath, new Uint8Array(res));
                return 0;
            }
        }
        return -1;
    }

    if (FS.analyzePath(backupPath).exists) {

        const waitFlush = new Promise((done, _) => {
            setTimeout(done, 10);
        });

        await waitFlush;

        const data = FS.readFile(backupPath);

        const blob = new Blob([data], {
            type: 'application/octet-stream',
            ok: true,
            status: 200
        });

        const headers = new Headers({
            'content-length': blob.size
        });

        const response = new Response(blob, {
            headers
        });

        await db.cache.put(cachePath, response);

        FS.unlink(backupPath);

        return 1;
    }
    return -1;  
}

export async function generateDownloadLink(parent, file) {

    const backupPath = `${file}`;
    const cachePath = `/data/cache/${backupPath.split('.')[0]}.db`;
    const db = window.sqlitedb;
    const resp = await db.cache.match(cachePath);

    if (resp && resp.ok) {

        const res = await resp.blob();
        if (res) {
            const a = document.createElement("a");
            a.href = URL.createObjectURL(res);
            a.download = backupPath;
            a.target = "_self";
            a.innerText = `Download ${backupPath}`;
            parent.innerHTML = '';
            parent.appendChild(a);
            return true;
        }
    }

    return false;
}
