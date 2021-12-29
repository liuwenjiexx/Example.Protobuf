local pb = require 'pb'


---@class Proto
local Proto = {}

local protoCSCmd
local protoSCCmd
local decodeed = true;

local default_cache_tb = {}
local __decode = pb.decode

local __G_ERROR_TRACK = __G_ERROR_TRACK

local decode = function(typename, buffer)
    if nil ~= buffer then
        local ret = __decode(typename, buffer);
        if false == ret then
            print(string.format("<color = #FF0000>%s</color>", err .. "\t" .. typename));
            decodeed = false;
            return false;
        end
        return ret
    end
    --default data
    local def = default_cache_tb[typename];
    if nil == def then
        def = pb.default(typename, {});
        default_cache_tb[typename] = def;
    end
    return def;
end

function Proto:initialize(options)
    protoCSCmd = options.CSCmd
    protoSCCmd = options.SCCmd
    pb.load(options.bytes)

end

function Proto:encode(type, name, data)
    local p = nil
    local msgId = nil
    local errorMsg = nil

    if type == "cs" then
        local msg = protoCSCmd.nameToMsg[name]
        if msg == nil then
            error("serialize cmd is null > " .. name)
        else
            p, errorMsg = pb.encode(msg, data)
            msgId = protoCSCmd.nameToId[name]
        end
    elseif type == "sc" then
        local msg = protoSCCmd.nameToMsg[name]
        if msg == nil then
            error("serialize cmd is null > " .. name)
        else
            p, errorMsg = pb.encode(msg, data)
            msgId = protoCSCmd.nameToId[name]
        end
    else
        error("serialize type is error > " .. type)
    end
    if errorMsg then
        error("serialize type is error > " .. name .. "  " .. errorMsg)
    end
    return p, msgId
end

function Proto:decode(type, name, data)
    local p = nil
    local ok, err = xpcall(function()
        if type == "cs" then
            local msg = protoCSCmd.nameToMsg[name]
            if msg == nil then
                error("serialize name is null > " .. name)
            else
                p = decode(msg, data)
            end
        elseif type == "sc" then
            local msg = protoSCCmd.nameToMsg[name]
            if msg == nil then
                error("serialize name is null > " .. name)
            else
                p = decode(msg, data)
            end
        else
            error("serialize type is error > " .. type)
        end
    end, function()
        if __G_ERROR_TRACK then
            __G_ERROR_TRACK(name)
        else
            print("ERROR", debug.traceback(name))
        end
    end, 33)
    if not ok then
        if __G_ERROR_TRACK then
            __G_ERROR_TRACK(name)
        else
            print("ERROR", debug.traceback(name))
        end
    else
        return p
    end
end

-- CS encode
function Proto:cs(name, data)
    local bytes = nil
    local msgId = nil
    local errorMsg = nil

    local msg

    msg = protoCSCmd.nameToMsg[name]
    if msg == nil then
        error("serialize name is null > " .. name)
    else
        bytes, errorMsg = pb.encode(msg, data)
        msgId = protoCSCmd.nameToId[name]
    end
    return bytes, msgId
end

function Proto:sc(name, data)
    local v = nil
    local msg
    if type(name) == "number" then
        msg = protoSCCmd.idToMsg[name]
    else
        msg =  protoSCCmd.nameToMsg[name]
    end
    if msg == nil then
        error("serialize name is null > " .. name)
    else
        v = decode(msg, data)
    end
    return v
end

function Proto:getCSId(name)
    return protoCSCmd.nameToId[name]
end

function Proto:getSCId(name)
    return protoSCCmd.nameToId[name]
end

return Proto