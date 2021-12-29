-- ***该文件为自动生成的***
    
local package = "Example"
local m = {
      [1] = package .. ".CS_Login",
      [2] = package .. ".CS_Login2",
      [3] = package .. ".SC_Login"
}

return {
    cs = {
        idToMsg = {
            [10001] = m[1],
            [10002] = m[2]
        },
        idToName = {
            [10001] = "CS_Login",
            [10002] = "CS_Login2"
        },
        nameToMsg = {
            ["Login"] = m[1],
            ["Login2"] = m[2]
        },
        nameToId = {
            ["Login"] = 10001,
            ["Login2"] = 10002
        }
    },
    sc = {
        idToMsg = {
            [10002] = m[3]
        },
        idToName = {
            [10002] = "SC_Login"
        },
        nameToMsg = {
            ["Login"] = m[3]
        },
        nameToId = {
            ["Login"] = 10002
        }
    }
}

  