#UTLWSPC.jcl  monthly spousal consol report
#
#set environment variables

 . /uheaa/whse/copy_lib/sasprofile
 . /uheaa/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWSPC.LWSPCR1
   then
        rm ${reportdir}/ULWSPC.LWSPCR1
fi
if test -a ${reportdir}/ULWSPC.LWSPCR2
   then
        rm ${reportdir}/ULWSPC.LWSPCR2
fi

# run the program

sas ${codedir}/UTLWSPC.sas -log ${reportdir}/ULWSPC.LWSPCR1  -mautosource
