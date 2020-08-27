import React, {useState, useEffect} from 'react';


export function SortName() {
    const [isFormValid, setIsFormValid] = useState(false);
    const [fileContent, setFileContent] = useState('');
    const [sortedNames, setSortedNames] = useState([]);
    const [downloadBtnDisable, setDownloadBtnDisable] = useState(true);

    async function sortNames() {

        let url = 'Name/Sort';
        let request = {
            "nameContent": fileContent,
            "sortType": 'LastNameThenGivenName'
        };

        console.log(fileContent);

        await fetch(url, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(request)
        }).then(async function (response) {
            if (response.ok) {
                let data = await response.json();
                setSortedNames(data.sortedNames);
                handleDownloadButtonDisable();
            } else {
                console.log(response);
                alert(response.statusText);
                loadForm();
            }
        });
    }

    useEffect(() => {
        loadForm();
    }, []);


    useEffect(() => {
        let isFormValid = fileContent!=='' && downloadBtnDisable;
        setIsFormValid(isFormValid);

    }, [fileContent,downloadBtnDisable]);

    async function download() {

        let url = 'Name/Download?'
            + `nameContent=${encodeURIComponent(sortedNames.join("\n"))}`;

        window.location.replace(url);

        loadForm();
    }

    const handleFileSelection = (event) => {
        event.preventDefault()
        let file = event.target.files[0];
        console.log(file);
        if (file.size > 2097152) {
            alert("File cannot be bigger than 2MB!");
            event.target.value = null;
        } else {
            const reader = new FileReader();
            reader.onload = (event) => {
                const text = (event.target.result);
                console.log(text)
                setFileContent(typeof text === 'string' ? text : ArrayBuffer.from(text).toString());
                console.log(fileContent)
            };
            reader.readAsText(file);
        }
    };

    function loadForm() {
        setFileContent('');
        setSortedNames([]);
        setDownloadBtnDisable(true);
    }

    const handleDownloadButtonDisable = () => {
        if (sortedNames === '') {
            setDownloadBtnDisable(true);
        } else {
            setDownloadBtnDisable(false);
        }
        console.log(downloadBtnDisable);
    };

    return <div>
        <h2>Name Sorter</h2>
        <form>
            <div className="form-group">
                <label id="name_sorter_file_selection_label_id">Please select file(*only support .txt file which is less
                    than 2MB.)</label>
                <input id="name_sorter_file_selection_id"
                       type="file"
                       className="form-control-file border"
                       accept=".txt"
                       required
                       onChange={handleFileSelection}/>
            </div>
            <div>
                <button type="button" className="btn btn-primary" onClick={sortNames}
                        disabled={!isFormValid}>Sort
                </button>
            </div>
            <div>
                <label id="name_sorter_display_label_id">Sorted Name:</label>
                <br/>
                <textarea id="name_sorter_display_textarea_id" rows="10" cols="50"
                          value={sortedNames.join("\n")}
                          readOnly/>
                <br/>
                <button type="button" disabled={downloadBtnDisable} className="btn btn-primary"
                        onClick={download}>Download
                </button>
            </div>
        </form>
    </div>
}
