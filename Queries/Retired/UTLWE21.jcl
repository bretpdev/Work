#UTLWE21.jcl 
#
#set environment variables

 . /uheaa/whse/copy_lib/sasprofile
 . /uheaa/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWE21.LWE21R1
   then
        rm ${reportdir}/ULWE21.LWE21R1
fi
if test -a ${reportdir}/ULWE21.LWE21R2
   then
        rm ${reportdir}/ULWE21.LWE21R2
fi
if test -a ${reportdir}/ULWE21.LWE21R3
   then
        rm ${reportdir}/ULWE21.LWE21R3
fi

# run the program

sas ${codedir}/UTLWE21.sas -log ${reportdir}/ULWE21.LWE21R1  -mautosource
