-- ***该文件为自动生成的***
    
local package = "Example"
local m = {
      [1] = package .. ".CS_Login",
      [2] = package .. ".SC_Login"
}

return {
    cs = {
        idToMsg = {
            [10001] = m[1]
        },
        idToName = {
            [10001] = "CS_Login"
        },
        nameToMsg = {
            ["Login"] = m[1]
        },
        nameToId = {
            ["Login"] = 10001
        }
    },
    sc = {
        idToMsg = {
            [10002] = m[2]
        },
        idToName = {
            [10002] = "SC_Login"
        },
        nameToMsg = {
            ["Login"] = m[2]
        },
        nameToId = {
            ["Login"] = 10002
        }
    }
}

  